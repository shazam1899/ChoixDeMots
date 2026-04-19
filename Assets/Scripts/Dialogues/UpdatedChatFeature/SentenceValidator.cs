using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SentenceValidator : MonoBehaviour
{
    private List<WordSlots> slots = new List<WordSlots>();
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    
    public void SetupSlots(List<WordSlots> newSlots)
    {
        slots = newSlots;
    }

    public void AddSlot(WordSlots slot)
    {
        slots.Add(slot);
    }

    public void CheckSentence()
    {
        //Making sure all slots are filled
        if (slots == null || slots.Count == 0) return;
        
        foreach (var slot in slots)
        {
            if (slot.currentWord == "")
                return; //not all slots are filled yet
        }

        //Get the index each slot's current word maps to
        int firstIndex = slots[0].GetCurrentIndex();

        //if any slots return -1 (word not in optons) or
        //if slots dont all share the same index, sentence is wrong
        if (firstIndex == -1)
        return;

        foreach (var slot in slots)
        {
            if (slot.GetCurrentIndex() != firstIndex)
            return;
        }

        //All slots match the same index, sentence is correct
        OnSentenceCorrect(firstIndex);
    }

    private void OnSentenceCorrect(int validatedIndex)
    {
        foreach (var slot in slots)
            slot.LockWord();

        dialogueManager.DestroyAllCubes();

        pendingIndex = validatedIndex;
        Invoke(nameof(CompleteWithDelay), 2f);
    }

    private int pendingIndex;

    private void CompleteWithDelay()
    {
        dialogueManager.OnPlayerTurnComplete(pendingIndex);
    }
}
