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
