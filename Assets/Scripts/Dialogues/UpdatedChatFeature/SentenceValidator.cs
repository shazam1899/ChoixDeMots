using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SentenceValidator : MonoBehaviour
{
    private List<WordSlots> slots = new List<WordSlots>();
    private DialogueData currentEntry;
    private DialogueManager dialogueManager;
    private PlayerMessage currentPlayerMessage;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    
    public void SetupSlots(List<WordSlots> newSlots, DialogueData entry, PlayerMessage playerMessage)
    {
        slots = newSlots;
        currentEntry = entry;
        currentPlayerMessage = playerMessage;
    }

    //called when a word is placed - checks for dynamic blank expansion
    public void OnWordPlaced(WordSlots filledSlot)
    {
        int placedIndex = filledSlot.GetCurrentIndex();
        if (placedIndex == -1)
        {
            CheckSentence();
            return;
        }

        //Count how many words in the dialogue share this index
        int totalWordsForIndex = CountWordsForIndex(placedIndex);

        //Count how many slots currently exist for index
        int currentSlotsForIndex = CountSlotsForIndex(placedIndex);

        //if we need more slots, spawn them
        if (currentSlotsForIndex < totalWordsForIndex)
        {
            int slotsToAdd = totalWordsForIndex - currentSlotsForIndex;
            dialogueManager.SpawnDynamicSlots(slotsToAdd, placedIndex, currentPlayerMessage);
        }

        CheckSentence();
    }

    private int CountWordsForIndex(int index)
    {
        //count how any entries in the full dialogue list reference this index
        int count = 0;
        foreach (var entry in dialogueManager.dialogueEntries)
        {
            if (entry.isPlayerTurn && entry.words != null)
            {
                foreach (var word in entry.words)
                {
                    if (word.isEmpty && word.optionIndices != null)
                    {
                        foreach (var idx in word.optionIndices)
                        {
                            if (idx == index)
                            {
                                count++;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return Mathf.Max(1, count);
    }

    private int CountSlotsForIndex(int index)
    {
        int count = 0;
        foreach (var slot in slots)
        {
            if (slot.linkedIndex == index)
                count++;
        }
        return count;
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

        dialogueManager.OnPlayerTurnComplete(validatedIndex);
    }
}
