using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WordCube : MonoBehaviour
{
    public TextMeshPro wordText;
    private string cubeWord;

    public void SetWord(string newWord)
    {
        cubeWord = newWord;
        if (wordText != null) wordText.text = newWord;
    }

    public string GetWord()
    {
        return cubeWord;
    }

    public void SetVisible(bool visible)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = visible;
        
    }
}
