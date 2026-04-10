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
    private XRSocketInteractor socket;
    private SentenceValidator validator;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        validator = FindFirstObjectByType<SentenceValidator>();

        socket.selectEntered.AddListener(OnWordPlaced);
        socket.selectExited.AddListener(OnWordRemoved);
    }

    private void OnWordPlaced(SelectEnterEventArgs args)
    {
        var cube = args.interactableObject.transform.GetComponent<WordCube>();
        if (cube != null)
        {
            currentWord = cube.GetWord();
            slotText.text = currentWord; //update UI blank 
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
        var cube = socket.GetOldestInteractableSelected().transform.GetComponent<XRGrabInteractable>();
        if (cube != null)
            cube.enabled = false;
    }
}
