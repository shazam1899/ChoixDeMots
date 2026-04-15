using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public List<TextMeshProUGUI> blankTexts; //assign in inspector
    
    public List<Vector3> BuildSentence(List<WordEntry> words)
    {
        List<Vector3> blankPositions = new List<Vector3>();
        
        //Build the visual sentence with fixed words and blanks
        string display = "";
        int blankIndex = 0;

        foreach (var word in words)
        {
            if (word.isEmpty)
            {
                display += "___"; //placeholder shown 
                if (blankIndex < blankTexts.Count)
                {
                    //Force update so position is correct
                    Canvas.ForceUpdateCanvases();
                    blankPositions.Add(blankTexts[blankIndex].transform.position);
                }
                blankIndex++;
            }
            else
            {
                display += word.word + " ";
            }
        }
        if (messageText != null)
            messageText.text = display.Trim();

        return blankPositions;
    }

    public void AddBlankAtPosition(int index, Vector3 worldPosition)
    {
        //called when dynamic blanks are added
        if (index < blankTexts.Count)
        {
            blankTexts[index].text = "___";
        }
    }

    public TextMeshProUGUI GetBlankText(int index)
    {
        if (blankTexts != null && index < blankTexts.Count)
            return blankTexts[index];
        return null;
    }

    public void UpdateBlankText(int index, string word)
    {
        if (index < blankTexts.Count)
            blankTexts[index].text = word;
    }
}
