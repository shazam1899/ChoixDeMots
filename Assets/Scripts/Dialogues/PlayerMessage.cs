using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject blankTextPrefab; //simple TextMeshProUGUI prefab
    public Transform wordsContainer; // horizontal layout group to hold words
    public List<TextMeshProUGUI> blankTexts = new List<TextMeshProUGUI>();
    
    public List<Vector3> BuildSentence(List<WordEntry> words)
    {
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
                blankTexts.Add(tmp);
            }
            else
            {
                tmp.text = word.word;
            }
        }
        //Force update so positions are correct
        Canvas.ForceUpdateCanvases();
        
        //collect positions of blank
        foreach (var blank in blankTexts)
            blankPositions.Add(blank.transform.position);

        return blankPositions;
    }

    public TextMeshProUGUI GetBlankText(int index)
    {
        if (index < blankTexts.Count)
            return blankTexts[index];
        return null;
    }
}
