using UnityEngine;

public class NPCMessage : MonoBehaviour
{
    public TMPro.TextMeshProUGUI senderText;
    public TMPro.TextMeshProUGUI messageText;

    public void SetMessage(string sender, string message)
    {
        senderText.text = sender;
        messageText.text = message;
    }

    public void SetIncompleteMessage(string message, string[] missingWords)
    {
       senderText.text = "YOU";
       messageText.text = message;
    }
}
