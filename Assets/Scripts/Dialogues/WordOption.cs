using UnityEngine;

public class WordOption : MonoBehaviour
{
    public TMPro.TextMeshProUGUI wordText;
    private string word;

    public void SetWord(string newWord)
    {
        word = newWord;
        wordText.text = word;
    }

    public string GetWord()
    {
        return word;
    }
}
