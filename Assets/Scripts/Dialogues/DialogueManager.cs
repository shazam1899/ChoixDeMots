using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject npcMessagePrefab;
    public GameObject playerMessagePrefab;
    public GameObject wordOptionPrefab;
    public Transform npcChatContainer;
    public Transform playerChatContainer;
    public Transform wordOptionsContainer;
    public Transform[] blankSlots; // snap zone for missing words

    public List<DialogueData> dialogueEntries; // List of dialogues to display in order
    private int currentIndex = 0;
    void Start()
    {
        ShowNextEntry();
    }

    public void ShowNextEntry()
    {
        if (currentIndex >= dialogueEntries.Count)
            return;

        var entry = dialogueEntries[currentIndex]; 

        if (!entry.isPlayerTurn)
        {
            //Spawn NPC Message
            var bubble = Instantiate(npcMessagePrefab, npcChatContainer);
            bubble.GetComponent<NPCMessage>().SetMessage(entry.senderName, entry.message);
            currentIndex++;
            //Automatically show next entry after a delay
            Invoke(nameof(ShowNextEntry), 0.5f);
        }

        else
        {
            //Show incomplete player message with blank slots
            var bubble = Instantiate(playerMessagePrefab, playerChatContainer);
            bubble.GetComponent<PlayerMessage>().SetIncompleteMessage(entry.message, entry.missingWords);

            //Spawn words for player
            foreach (var word in entry.wordOptions)
            {
                var option = Instantiate(wordOptionPrefab, wordOptionsContainer);
                option.GetComponent<WordOption>().SetWord(word);
            }
        }

        return;
    }
}
