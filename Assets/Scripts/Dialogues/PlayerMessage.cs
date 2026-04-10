using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessage : MonoBehaviour
{
    public TMPro.TextMeshProUGUI senderText;
    public TMPro.TextMeshProUGUI messageText;

    public List<TextMeshProUGUI> blankTexts; //assign in inspector

    public TextMeshProUGUI GetBlankText(int index)
    {
        if (index < blankTexts.Count)
            return blankTexts[index];
        return null;
    }

    public void BuildSentence(List<SentenceWord> words)
    {
        //Build the visuaæ sentence with fixed words and blanks
        string display = "";
        int blankIndex = 0;
        foreach (var word in words)
        {
            if (word.isBlank)
            {
                display += "___"; //placeholder shown 
                blankIndex++;
            }
            else
            {
                display += word.fixedWord + " ";
            }
        }
        messageText.text = display.Trim();
    }

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
