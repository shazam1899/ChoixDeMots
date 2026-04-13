using UnityEngine;
using System.Collections.Generic;

public class NPCMessage : MonoBehaviour
{
    public TMPro.TextMeshProUGUI senderText;
    public TMPro.TextMeshProUGUI messageText;
 public void BuildSentence(List<WordEntry> words)
    {
        //Build the visual sentence with fixed words and blanks
        string display = "";
        int blankIndex = 0;
        foreach (var word in words)
        {
            if (word.isEmpty)
            {
                display += "___"; //placeholder shown 
                blankIndex++;
            }
            else
            {
                display += word.word + " ";
            }
        }
        messageText.text = display.Trim();
    }

}
