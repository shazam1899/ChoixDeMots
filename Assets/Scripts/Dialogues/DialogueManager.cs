using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject npcMessagePrefab;
    public GameObject playerMessagePrefab;
    public GameObject wordCubePrefab;
    public Transform npcChatContainer;
    public Transform playerChatContainer;
    public Transform wordOptionsContainer;
    public Transform[] blankSlots; // snap zone for missing words

    public GameObject wordSlotPrefab; //prefab xrsocketinteractor
    public Transform slotSpawnPoint; //where slots appear in 3D space
    public float slotSpacing = 0.3f; //space between slots

    private List<WordSlots> activeSlots = new List<WordSlots>();

    public Transform cubeSpawnPoint; 

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
                var cube = Instantiate(wordCubePrefab, cubeSpawnPoint.position, cubeSpawnPoint.rotation);
                cube.GetComponent<WordCube>().SetWord(word);
            }
        }

        return;
    }

    public void OnPlayerTurnComplete(int nextIndex)
    {
        currentIndex = nextIndex;
        ShowNextEntry();
    }

    private void SpawnPlayerSentence(DialogueData entry)
    {
        activeSlots.Clear();
        var bubble = Instantiate(playerMessagePrefab, playerChatContainer);
        var messageBubble = bubble.GetComponent<PlayerMessage>();

        int blankIndex = 0;
        foreach (var word in entry.sentenceWords)
        {
            if (word.isBlank)
            {
                //spawn 3D snapzone for blank
                Vector3 slotPosition = slotSpawnPoint.position + Vector3.right * (blankIndex * slotSpacing);
                var slotObject = Instantiate(wordSlotPrefab, slotPosition, slotSpawnPoint.rotation);
                var slot = slotObject.GetComponent<WordSlots>();

                //tell the slot which UI text element to update
                slot.slotText = messageBubble.GetBlankText(blankIndex);
                activeSlots.Add(slot);
                blankIndex++;
            }
        }

        //pass active slots and outcomes to the validator
        var validator = FindFirstObjectByType<SentenceValidator>();
        validator.SetupSlots(activeSlots.ToArray(), entry.possibleOutcomes);
    }
}
