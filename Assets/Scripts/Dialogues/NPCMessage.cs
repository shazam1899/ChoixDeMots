using UnityEngine;
using System.Collections.Generic;

public class NPCMessage : MonoBehaviour
{
    public TMPro.TextMeshProUGUI senderText;
    public TMPro.TextMeshProUGUI messageText;
    public TMPro.TextMeshProUGUI messageTextVisible;
 public void SetMessage(string sender, string message, string messageVisible)
    {
        if (senderText != null) senderText.text = sender;
        if (messageText != null) messageText.text = message;
        if (messageTextVisible != null) messageTextVisible.text = messageVisible;
    }

}
