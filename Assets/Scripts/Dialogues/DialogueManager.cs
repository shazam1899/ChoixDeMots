using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject npcMessagePrefab;
    public GameObject playerMessagePrefab;
    public GameObject wordCubePrefab;
    public GameObject wordSlotPrefab; //prefab xrsocketinteractor
    public Transform ChatContainer;
    public Transform cubeSpawnPoint;
    public Transform slotSpawnPoint; //where slots appear in 3D space
    public float slotSpacing = 0.3f; //space between slots
    
    public List<DialogueData> dialogueEntries; // List of dialogues to display in order
    private int currentIndex = 0;
    private List<WordSlots> activeSlots = new List<WordSlots>();

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
            string message = BuildSentence(entry.words);
            var bubble = Instantiate(npcMessagePrefab, ChatContainer);
            bubble.GetComponentInChildren<NPCMessage>().BuildSentence(entry.words);
            currentIndex++;
            //Automatically show next entry after a delay
            Invoke(nameof(ShowNextEntry), 0.5f);
        }

        else
        {
            //Show incomplete player message with blank slots
            //var bubble = Instantiate(playerMessagePrefab, ChatContainer);
            //bubble.GetComponentInChildren<PlayerMessage>().BuildSentence(entry.words);

            //Spawn words for player
            //foreach (var word in entry.words)
            //{
                //foreach (var cubeWord in word.options)
                //{
                    //var cube = Instantiate(wordCubePrefab, cubeSpawnPoint.position, cubeSpawnPoint.rotation);
                    //cube.GetComponent<WordCube>().SetWord(cubeWord); 
                //}
                
            //}

            SpawnPlayerSentence(entry);
        }
    }

    private string BuildSentence(List<WordEntry> words)
    {
        string sentence = "";
        foreach (var word in words)
        {
            sentence += word.isEmpty ? "___" : word.word + " ";
        }
        return sentence.Trim();
    }
    private void SpawnPlayerSentence(DialogueData entry)
    {
        activeSlots.Clear();

        //Spawn player bubble
        var bubble = Instantiate(playerMessagePrefab, ChatContainer);
        var playerMessage = bubble.GetComponentInChildren<PlayerMessage>();
        playerMessage.BuildSentence(entry.words);

        int blankIndex = 0;
        foreach (var word in entry.words)
        {
            if (word.isEmpty)
            {
                //spawn 3D snapzone for blank
                Vector3 slotPosition = slotSpawnPoint.position + Vector3.right * (blankIndex * slotSpacing);
                var slotObject = Instantiate(wordSlotPrefab, slotPosition, slotSpawnPoint.rotation);
                var slot = slotObject.GetComponent<WordSlots>();

                //tell the slot which UI text element to update
                slot.slotText = playerMessage.GetBlankText(blankIndex);
                
                slot.SetOptions(word.options, word.optionIndices);

                activeSlots.Add(slot);
                blankIndex++;
            }
        }

        //Spawn word cubes for all unique options
        List<string> allOptions = new List<string>();
        foreach (var word in entry.words)
        {
            if (word.isEmpty && word.options != null)
            {
                foreach (var option in word.options)
                {
                    if (!allOptions.Contains(option))
                        allOptions.Add(option);
                }
            }
        }



        for (int i = 0; i < allOptions.Count; i++)
            {
            Vector3 cubePosition = cubeSpawnPoint.position + Vector3.right * (i * slotSpacing);
            var cube = Instantiate(wordCubePrefab, cubePosition, cubeSpawnPoint.rotation);
            cube.GetComponent<WordCube>().SetWord(allOptions[i]);
            }

            //Pass through validator
            FindFirstObjectByType<SentenceValidator>().SetupSlots(activeSlots.ToArray());
    }
    public void OnPlayerTurnComplete(int nextIndex)
    {
        currentIndex = nextIndex;
        ShowNextEntry();
    }
        //pass active slots and outcomes to the validator
        //var validator = FindFirstObjectByType<SentenceValidator>();
        //validator.SetupSlots(activeSlots.ToArray(), entry.possibleOutcomes);
}
