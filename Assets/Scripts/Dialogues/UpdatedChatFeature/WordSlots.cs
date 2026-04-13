using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WordSlots : MonoBehaviour
{
    public TextMeshProUGUI slotText; //blank space in the player bubble UI
    public string currentWord = "";
    private string[] options;
    private int[] optionIndices;
    private XRSocketInteractor socket;
    private SentenceValidator validator;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        validator = FindFirstObjectByType<SentenceValidator>();

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
            if (options[i].ToLower() == currentWord.ToLower())
                return optionIndices[i];
        }
        return -1;
    }
    
    private void OnWordPlaced(SelectEnterEventArgs args)
    {
        var cube = args.interactableObject.transform.GetComponent<WordCube>();
        if (cube != null)
        {
            currentWord = cube.GetWord();
            if (slotText != null) slotText.text = currentWord; //update UI blank 
            validator.CheckSentence();
        }
    }

    private void OnWordRemoved(SelectExitEventArgs args)
    {
        currentWord = "";
        slotText.text = "___"; //reset UI blank
        validator.CheckSentence();
    }

    public void LockWord()
    {
        var interactable = socket.GetOldestInteractableSelected()?.transform.GetComponent<XRGrabInteractable>();
        if (interactable != null)
            interactable.enabled = false;
    }
}
