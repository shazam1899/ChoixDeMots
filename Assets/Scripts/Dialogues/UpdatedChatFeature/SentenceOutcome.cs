using UnityEngine;

[System.Serializable]
public class SentenceOutcome
{
    public string[] words; //the words 
    public int nextDialogueIndex; //which NPC response to jump to
}
