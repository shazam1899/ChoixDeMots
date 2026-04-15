using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WordEntry
{
    public string word; //fixed word, empty if blank
    public bool isEmpty; // is blank spot?
    public string[] options; //only used if isEmpty is true
    public int[] optionIndices; //which dialogue index each option leads to
    public int linkedIndex; //which index blank belongs to
}
[System.Serializable]
public class DialogueData
{
    public string senderName;
    public bool isPlayerTurn;
    public List<WordEntry> words; //each word in the sentence
}
