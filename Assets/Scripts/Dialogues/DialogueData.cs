using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WordEntry
{
    public string word; //fixed word, empty if blank
    public bool isEmpty; // is blank spot?
    public bool isIndexed; //does this blank affect which index plays ?
    public string[] options; //only used if isEmpty is true
    public int[] optionIndices; //which dialogue index each option leads to
}
[System.Serializable]
public class DialogueData
{
    public string senderName;
    public bool isPlayerTurn;
    public bool isDialogueEnd; //stops dialogue when entry plays
    public List<WordEntry> words; //each word in the sentence
    public GameObject messageAnimation; //animation to play when this message displays
    public GameObject bodyAnimation; 
    public bool playerBlocked; //is the player blocked ? 
    public bool donneReward; // does the message give you a reward ? 
}
