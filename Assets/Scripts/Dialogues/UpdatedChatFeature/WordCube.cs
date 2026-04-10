using TMPro;
using UnityEngine;

public class WordCube : MonoBehaviour
{
    public TextMeshPro wordText;
    private string word;

    public void SetWord(string newWord)
    {
        word = newWord;
        wordText.text = newWord;
    }

    public string GetWord()
    {
        return word;
    }
}
