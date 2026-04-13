using System;
using TMPro;
using UnityEngine;

public class WordCube : MonoBehaviour
{
    public TextMeshPro wordText;
    private string cubeWord;

    public void SetWord(string newWord)
    {
        cubeWord = newWord;
        wordText.text = newWord;
    }

    public string GetWord()
    {
        return cubeWord;
    }
}
