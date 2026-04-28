using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WordSlots : MonoBehaviour
{
    public TextMeshProUGUI slotText; //blank space in the player bubble UI
    public string currentWord;
    private string[] options;
    private int[] optionIndices;
    private XRSocketInteractor socket;
    private SentenceValidator validator;
    public bool isFirstSlot = false;
    public bool isIndexed = false;

    //called manually after creation instead of start
    public void Initialize(SentenceValidator sentenceValidator)
    {
        validator = sentenceValidator;
        socket = GetComponent<XRSocketInteractor>();

        if (socket == null)
        {
            Debug.LogError("no xrsocketinteractor found on wordslot bro!");
            return;
        }

        socket.selectEntered.AddListener(OnWordPlaced);
        socket.selectExited.AddListener(OnWordRemoved);
    }

    public void SetOptions(string[] newOptions, int[] newIndices)
    {
        options = newOptions;
        optionIndices = newIndices;
    }

    public int GetCurrentIndex()
    {
        //Find which index the current word maps to
        if (options == null || currentWord == "") return -1;
        for (int i = 0; i < options.Length; i++)
        {
            Debug.Log($"Comparing '{options[i].ToLower()}' with '{currentWord.ToLower()}'");
            if (options[i].ToLower() == currentWord.ToLower())
                return optionIndices[i];
        }
        Debug.Log("No match found bro");
        return -1;
    }
    
    private void OnWordPlaced(SelectEnterEventArgs args)
    {
        var cube = args.interactableObject.transform.GetComponent<WordCube>();
        if (cube != null)
        {
            currentWord = cube.GetWord();
            if (slotText != null) slotText.text = currentWord; //update UI blank 
            
            //hide the cube visibility
            cube.SetVisible(false);

            if (isFirstSlot)
                validator.OnFirstWordPlaced(this);
            else
            // Tell validator a word was placed — may trigger dynamic blank spawning
            validator.CheckSentence();
        }
    }

    private void OnWordRemoved(SelectExitEventArgs args)
    {
        var cube = args.interactableObject.transform.GetComponent<WordCube>();
        if (cube != null)
        {
            //Show the cube again
            cube.SetVisible(true);
        }
        
        currentWord = "[...]";
        if (slotText != null) slotText.text = "[...]"; //reset UI blank
        
        if (isFirstSlot)
            validator.OnFirstWordRemoved();
        else
            validator.CheckSentence();
    }

    public void LockWord()
    {
        var cube = socket.GetOldestInteractableSelected()?.transform.GetComponent<WordCube>();
        if (cube != null)
        {
            cube.SetVisible(false);
            var interactable = cube.GetComponent<XRGrabInteractable>();
            if (interactable != null)
                interactable.enabled = false;
        }

        //show word permanently
        if (slotText != null)
            slotText.text = currentWord;
            
    }
}
