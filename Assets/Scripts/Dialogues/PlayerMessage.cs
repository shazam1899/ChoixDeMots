using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI messageText;
    public GameObject blankTextPrefab; //simple TextMeshProUGUI prefab
    public Transform wordsContainer; // horizontal layout group to hold words
    public List<TextMeshProUGUI> blankTexts = new List<TextMeshProUGUI>();
    
    public List<Vector3> BuildSentence(List<WordEntry> words, string senderName)
    {
        //Set sender name
        if (senderText != null) senderText.text = senderName; 
        
        blankTexts.Clear();
        List<Vector3> blankPositions = new List<Vector3>();
        
        //clear existing children
        foreach (Transform child in wordsContainer)
            Destroy(child.gameObject);

        foreach (var word in words)
        {
            //create a text element for each word
            var wordObject = Instantiate(blankTextPrefab, wordsContainer);
            var tmp = wordObject.GetComponent<TextMeshProUGUI>();
            
            if (word.isEmpty)
            {
                tmp.text = "___";
                wordObject.SetActive(false);
                blankTexts.Add(tmp);
            }
            else
            {
                tmp.text = word.word + " ";
            }
        }

        //show only first blank initially
        if (blankTexts.Count > 0)
            blankTexts[0].gameObject.SetActive(true);
        
        //Force update so positions are correct
        Canvas.ForceUpdateCanvases();
        
        //collect positions of blank
        if (blankTexts.Count > 0)
            blankPositions.Add(blankTexts[0]. transform.position);

        return blankPositions;
    }

    public Vector3 RevealAndGetPosition(int index)
    {
        if (index >= blankTexts.Count) return Vector3.zero;

        //reveal blank
        blankTexts[index].gameObject.SetActive(true);
        return blankTexts[index].transform.position;
    }

    public void RevealBlank(int index)
    {
        if (index < blankTexts.Count)
        {
            blankTexts[index].gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
        }
    }

    public TextMeshProUGUI GetBlankText(int index)
    {
        if (index < blankTexts.Count)
            return blankTexts[index];
        return null;
    }
}
