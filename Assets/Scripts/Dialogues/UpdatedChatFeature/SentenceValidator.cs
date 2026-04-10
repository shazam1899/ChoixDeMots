using UnityEngine;

public class SentenceValidator : MonoBehaviour
{
    public WordSlots[] slots;
    public SentenceOutcome[] possibleOutcomes;

    public void CheckSentence()
    {
        //Making sure all slots are filled
        foreach (var slot in slots)
        {
            if (slot.currentWord == "")
                return; //not all slots are filled yet
        }

        //Build current sentence from slot words
        string[] currrentWords = new string[slots.Length];
        for (int i = 0; i < slots.Length; i++)
            currrentWords[i] = slots[i].currentWord.ToLower();

        //Check against all possible outcomes
        foreach (var outcome in possibleOutcomes)
        {
            if (SentenceMatches(currrentWords, outcome.words))
            {
                OnSentenceCorrect(outcome);
                return;
            }
        }
        //no match found, player keeps going
    }

    private bool SentenceMatches(string[] current, string[] correct)
    {
        if (current.Length != correct.Length) return false;
        for (int i = 0; i < current.Length; i++)
        {
            if (current[i] != correct[i].ToLower())
                return false;
        }
        return true;
    }

    private void OnSentenceCorrect(SentenceOutcome outcome)
    {
        foreach (var slot in slots)
            slot.LockWord();

        FindFirstObjectByType<DialogueManager>().OnPlayerTurnComplete(outcome.nextDialogueIndex);
    }

    public void SetupSlots(WordSlots[] newSlots, SentenceOutcome[] newOutcomes)
    {
        slots = newSlots;
        possibleOutcomes = newOutcomes;
    }
}
