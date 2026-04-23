using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

public class SentenceValidator : MonoBehaviour
{
    private List<WordSlots> slots = new List<WordSlots>();
    private int pendingIndex;
    private DialogueManager dialogueManager;
    private bool isExpanded = false;
    private int lastExpandedIndex = -1;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    
    public void SetupSlots(List<WordSlots> newSlots)
    {
        slots = newSlots;
        isExpanded = false;
        lastExpandedIndex = -1;
    }

    public void AddSlot(WordSlots slot)
    {
        slots.Add(slot);
    }

    public void OnFirstWordPlaced(WordSlots firstSlot)
    {
        int placedIndex = firstSlot.GetCurrentIndex();
        if (placedIndex == -1) return;

        int totalNeeded = dialogueManager.CountWordsForIndex(placedIndex);

        if (isExpanded && lastExpandedIndex != placedIndex)
            
            CollapseToFirstSlot();
            isExpanded = false;

        if (!isExpanded && totalNeeded > 1)
        {
            dialogueManager.SpawnDynamicSlots(totalNeeded - 1, placedIndex);
            isExpanded = true;
            lastExpandedIndex = placedIndex;
        }

        CheckSentence();
    
    }

    public void OnFirstWordRemoved()
    {
        if (isExpanded)
            CollapseToFirstSlot();

        isExpanded = false;
        lastExpandedIndex = -1;
    }

    private void CollapseToFirstSlot()
    {
        string firstWord = slots.Count > 0 ? slots[0].currentWord : "";
        
        List<WordSlots> toRemove = new List<WordSlots>();
        //keep only first slot
        for (int i = slots.Count - 1; i > 0; i--)
            toRemove.Add(slots[i]);

        foreach (var slot in toRemove)
        {
            if (slot != null)
            {
                if (slot.slotText != null)
                {
                   slot.slotText.text = "[...]";
                   slot.slotText.gameObject.SetActive(false); //hide blank text 
                }
                Destroy(slot.gameObject);
            }
            slots.Remove(slot);  
        }

        if (slots.Count > 0 && slots[0].slotText != null)
            slots[0].slotText.text = firstWord;
    }

    public void CheckSentence()
    {
        //Making sure all slots are filled
        if (slots == null || slots.Count == 0) return;

        //get first filled slot's index
        int firstIndex = -1;
        foreach (var slot in slots)
        {
            if (slot.currentWord != "")
            {
                firstIndex = slot.GetCurrentIndex();
                break;
            }
        }
        
        if (firstIndex == -1) return;

        //make sure we have all slots needed before validating
        int totalNeeded = dialogueManager.CountWordsForIndex(firstIndex);
        if (slots.Count < totalNeeded) return;
        
        List<WordSlots> slotsCopy = new List<WordSlots>(slots);
        
        //dont validate if we're expanding slots
        if (isExpanded == false && slotsCopy.Count == 1)
        {
            //only one slot exists, check if more needed
            int singleSlotIndex = slotsCopy[0].GetCurrentIndex();
            if (singleSlotIndex != -1 && dialogueManager.CountWordsForIndex(singleSlotIndex) > 1)
                return; 
        }
        
        foreach (var slot in slotsCopy)
        {
            if (slot.currentWord == "")
                return; //not all slots are filled yet
        }

        foreach (var slot in slotsCopy)
        {
            if (slot.GetCurrentIndex() != firstIndex) return;
        }

        //All slots match the same index, sentence is correct
        OnSentenceCorrect(firstIndex);
    }

    private void OnSentenceCorrect(int validatedIndex)
    {
        List<WordSlots> slotsCopy = new List<WordSlots>(slots);
        foreach (var slot in slotsCopy)
            slot.LockWord();

        dialogueManager.DestroyAllCubes();

        pendingIndex = validatedIndex;
        Invoke(nameof(CompleteWithDelay), 2f);
    }

    private void CompleteWithDelay()
    {
        dialogueManager.OnPlayerTurnComplete(pendingIndex);
    }
    
}
