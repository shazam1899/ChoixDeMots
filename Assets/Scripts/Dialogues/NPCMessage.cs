using UnityEngine;
using System.Collections.Generic;

public class NPCMessage : MonoBehaviour
{
    public TMPro.TextMeshProUGUI senderText;
    public TMPro.TextMeshProUGUI messageText;
 public void SetMessage(string sender, string message)
    {
        if (senderText != null) senderText.text = sender;
        if (messageText != null) messageText.text = message;
    }

}
