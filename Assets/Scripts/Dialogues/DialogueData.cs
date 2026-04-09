using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string senderName;
    public string message;
    public bool isPlayerTurn;
    public string[] missingWords; //words to fill in
    public string[] wordOptions; //grabbable options to choose from
}
