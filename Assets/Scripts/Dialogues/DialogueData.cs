using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string senderName;
    public string message;
    public string[] missingWords;

    public bool isPlayerTurn;
    public List<SentenceWord> sentenceWords; //replace message string
    public string[] wordOptions;
    public SentenceOutcome[] possibleOutcomes;
}
