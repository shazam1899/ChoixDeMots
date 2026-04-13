using UnityEngine;

public class SentenceValidator : MonoBehaviour
{
    public WordSlots[] slots;

    public void SetupSlots(WordSlots[] newSlots)
    {
        slots = newSlots;
    }

    public void CheckSentence()
    {
        //Making sure all slots are filled
        if (slots == null || slots.Length == 0) return;
        
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

    private void OnSentenceCorrect(int nextIndex)
    {
        foreach (var slot in slots)
            slot.LockWord();

        FindFirstObjectByType<DialogueManager>().OnPlayerTurnComplete(nextIndex);
    }
}
