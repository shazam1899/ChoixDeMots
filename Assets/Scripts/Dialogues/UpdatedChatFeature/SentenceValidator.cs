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
        if (firstSlot.isIndexed)
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
        string firstWord = slots.Count > 0 ? slots[0].currentWord : "[...]";
        
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
        if (slots == null || slots.Count == 0)
        {
            Debug.Log("CheckSentence: No slots, returning");
            return;
        }

        //all slots must be filled regardless of isIndexed
        bool allSlotsFilled = true;
        foreach (var slot in slots)
        {
            if (slot.currentWord == "")
            {
                allSlotsFilled = false;
                break;
            }
        }

        Debug.Log($"CheckSentence: All slots filled = {allSlotsFilled}, slot count = {slots.Count}");

        if (!allSlotsFilled) 
        {
            Debug.Log("CheckSentence: Not all slots filled, returning without feedback");
            return;
        }
        
        //get first filled slot's index
        int firstIndex = -1;
        foreach (var slot in slots)
        {
            if (slot.isIndexed)
            {
                firstIndex = slot.GetCurrentIndex();
                Debug.Log($"CheckSentence: Found indexed slot with index {firstIndex}, word='{slot.currentWord}'");
                break;
            }
        }
        
        /*if (firstIndex == -1)
        {
            Debug.Log("CheckSentence: No valid firstIndex found, showing wrong feedback");
            //all filled but no valid index found - show feedback
            dialogueManager.currentPlayerMessage.ShowWrongFeedback();
            return;
        }*/

        //count indexed slots needed
        int indexedSlotsNeeded = dialogueManager.CountWordsForIndex(firstIndex);
        Debug.Log($"CheckSentence: Indexed slots needed for index {firstIndex} = {indexedSlotsNeeded}");

        //count non indexed slots
        int nonIndexedSlots = 0;
        foreach (var slot in slots)
        {
            if (!slot.isIndexed)
                nonIndexedSlots++;
        }
        Debug.Log($"CheckSentence: Non-indexed slots = {nonIndexedSlots}");

        //make sure we have all slots needed before validating
        int totalNeeded = indexedSlotsNeeded + nonIndexedSlots;
        Debug.Log($"CheckSentence: Total slots needed = {totalNeeded}, current slots = {slots.Count}");
        if (slots.Count < totalNeeded)
        {
            Debug.Log("CheckSentence: Not enough slots, returning");
            return;
        }
            
        List<WordSlots> slotsCopy = new List<WordSlots>(slots);

        bool isCorrect = true;
        foreach (var slot in slotsCopy)
        {
            if (slot.isIndexed && slot.GetCurrentIndex() != firstIndex)
            {
                //all filled but indexed slots dont match -- show feedback
                isCorrect = false;
                break;
            }
        }

        if (!isCorrect)
        {
            dialogueManager.currentPlayerMessage.ShowWrongFeedback();
            return;
        }

        //all non indexed slots just need any valid option
        foreach (var slot in slotsCopy)
        {
            if (!slot.isIndexed && slot.currentWord == "") return;
        }
        
        Debug.Log("CheckSentence: All validation passed! Sentence is correct");
        //All slots match the same index, sentence is correct
        OnSentenceCorrect(firstIndex);
    }

    private void OnSentenceCorrect(int validatedIndex)
    {
        //clear any wrong feedback before showing success
        dialogueManager.currentPlayerMessage.ClearWrongFeedback();
        
        List<WordSlots> slotsCopy = new List<WordSlots>(slots);
        foreach (var slot in slotsCopy)
            slot.LockWord();

        dialogueManager.DestroyAllCubes();

        pendingIndex = validatedIndex;
        Invoke(nameof(CompleteWithDelay), 0f);
    }

    private void CompleteWithDelay()
    {
        dialogueManager.OnPlayerTurnComplete(pendingIndex);
    }
    
}
