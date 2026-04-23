using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI messageText;
    public GameObject blankTextPrefab; //simple TextMeshProUGUI prefab
    public Transform wordsContainer; // horizontal layout group to hold words
    public List<TextMeshProUGUI> blankTexts = new List<TextMeshProUGUI>();

    //one hidden gameObject per possible outcome index
    private Dictionary<int, GameObject> sentenceVariants = new Dictionary<int, GameObject>();
    
    public List<Vector3> BuildSentence(List<WordEntry> words, string senderName)
    {
        //Set sender name
        if (senderText != null) senderText.text = senderName; 
        
        blankTexts.Clear();
        sentenceVariants.Clear();
        List<Vector3> blankPositions = new List<Vector3>();
        
        //clear existing children
        foreach (Transform child in wordsContainer)
            Destroy(child.gameObject);

        string display = "";
        foreach (var word in words)
            display += word.isEmpty ? "[...] " : word.word + " ";
        if (messageText != null)
            messageText.text = display.Trim();

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

    public void BuildSentenceVariants(List<WordEntry> words)
    {
        HashSet<int> allIndices = new HashSet<int>();
        foreach (var word in words)
        {
            if (word.isEmpty && word.optionIndices != null)
            {
                foreach (var idx in word.optionIndices)
                    allIndices.Add(idx);
            }
        }

        //for each possible index, build and hide a complete sentence
        foreach (var index in allIndices)
        {
            string variantText = "";
            foreach (var word in words)
            {
                if (word.isEmpty)
                {
                    string matchingWord = "";
                    if (word.options != null && word.optionIndices != null)
                    {
                        for (int i = 0; i < word.optionIndices.Length; i++)
                        {
                            if (word.optionIndices[i] == index && i < word.options.Length)
                            {
                                matchingWord = word.options[i];
                                break;
                            }
                        }
                    }
                    variantText += matchingWord + " ";
                }
                else
                {
                    variantText += word.word + " ";
                }
            }

            //creat hidden text object for this variant
            var variantObject = Instantiate(blankTextPrefab, wordsContainer);
            var tmp = variantObject.GetComponent<TextMeshProUGUI>();
            tmp.text = variantText.Trim();
            variantObject.SetActive(false); //hidden until validated
            sentenceVariants[index] = variantObject;
        }
    }

    //called on validation - hides placeholder, shows correct variant
    public void ShowValidatedSentence(int validatedIndex)
    {
        //hide placeholder
        if (messageText != null)
            messageText.gameObject.SetActive(false);

        //show correct variant
        if (sentenceVariants.ContainsKey(validatedIndex))
            sentenceVariants[validatedIndex].SetActive(true);

        //hide all other variants
        foreach (var kvp in sentenceVariants)
        {
            if (kvp.Key != validatedIndex)
                kvp.Value.SetActive(false);
        }

        //clean up anchors
        foreach (var blank in blankTexts)
        {
            if (blank != null)
            Destroy(blank.gameObject);
        }
        blankTexts.Clear();
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
