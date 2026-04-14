using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject npcMessagePrefab;
    public GameObject playerMessagePrefab;
    public GameObject wordCubePrefab;
    public GameObject wordSlotPrefab; //prefab xrsocketinteractor
    public Transform ChatContainer;
    public Transform[] cubeSpawnPoints;
    
    public List<DialogueData> dialogueEntries; // List of dialogues to display in order
    private int currentIndex = 0;
    private List<WordSlots> activeSlots = new List<WordSlots>();

    public ScrollRect chatScrollRect;

    void Start()
    {
        ShowNextEntry();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0f;
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
            bubble.GetComponentInChildren<NPCMessage>().SetMessage(entry.senderName, message);
            ScrollToBottom();
            currentIndex++;
            //Automatically show next entry after a delay
            Invoke(nameof(ShowNextEntry), 1.5f);
        }

        else
        {
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
        ScrollToBottom();
        playerMessage.BuildSentence(entry.words);

        if (!entry.words.Exists(w => w.isEmpty))
        {
            currentIndex++;
            Invoke(nameof(ShowNextEntry), 1.5f);
            return;
        }

        int blankIndex = 0;
        foreach (var word in entry.words)
        {
            if (word.isEmpty)
            {
                //spawn 3D snapzone for blank
                var slotObject = Instantiate(wordSlotPrefab, Vector3.zero, Quaternion.identity);
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
            Transform spawnPoint = cubeSpawnPoints[Random.Range(0, cubeSpawnPoints.Length)];
            var cube = Instantiate(wordCubePrefab, spawnPoint.position, spawnPoint.rotation);
            cube.GetComponent<WordCube>().SetWord(allOptions[i]);
            }

            //Pass through validator
            FindFirstObjectByType<SentenceValidator>().SetupSlots(activeSlots.ToArray());
    }
    public void OnPlayerTurnComplete(int nextIndex)
    {
        currentIndex = nextIndex;

        foreach (var slot in activeSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        activeSlots.Clear();

        ShowNextEntry();
    }
        //pass active slots and outcomes to the validator
        //var validator = FindFirstObjectByType<SentenceValidator>();
        //validator.SetupSlots(activeSlots.ToArray(), entry.possibleOutcomes);
}
