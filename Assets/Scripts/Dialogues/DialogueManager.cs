using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DialogueManager : MonoBehaviour
{
    public GameObject npcMessagePrefab;
    public GameObject playerMessagePrefab;
    public GameObject wordCubePrefab;
    public Transform ChatContainer;
    public Transform[] cubeSpawnPoints;
    public ScrollRect chatScrollRect;
    public float socketDepthOffset = 0.1f; //how far in front of UI the socket spawns

    //interaction layer for word cubes and slots must match
    public InteractionLayerMask wordInteractionLayer;
    
    public List<DialogueData> dialogueEntries; // List of dialogues to display in order
    public int currentIndex = 0;
    private List<WordSlots> activeSlots = new List<WordSlots>();
    public PlayerMessage currentPlayerMessage;
    private SentenceValidator validator;

    private List<GameObject> activeCubes = new List<GameObject>();
    public int pendingValidatedIndex = -1;
    

    void Start()
    {
        validator = FindFirstObjectByType<SentenceValidator>();
        ShowNextEntry();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        if (chatScrollRect != null)
            chatScrollRect.verticalNormalizedPosition = 0f;
    }

    public void ShowNextEntry()
    {
        if (currentIndex >= dialogueEntries.Count)
            return;

        if (IsReferencedIndex (currentIndex) && currentIndex != pendingValidatedIndex)
        {
            currentIndex++;
            ShowNextEntry();
            return;
        }
        
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

    private bool IsReferencedIndex(int index)
    {
        foreach (var entry in dialogueEntries)
        {
            if (entry.words == null) continue;
            foreach (var word in entry.words)
            {
                if (word.isEmpty && word.optionIndices != null)
                {
                    foreach (var idx in word.optionIndices)
                    {
                        if (idx == index) return true;
                    }
                }
            }
        }
        return false;
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

    public int CountWordsForIndex(int index)
    {
        var currentEntry = dialogueEntries[currentIndex];
        int count = 0;

        if (currentEntry.words == null) return 1;

        foreach (var word in currentEntry.words)
        {
            if (word.isEmpty && word.optionIndices != null)
            {
                foreach (var idx in word.optionIndices)
                {
                    if (idx == index)
                    {
                        count++;
                        break;
                    }
                }
            }
        }
        return Mathf.Max(1, count);
    }

    public void SpawnDynamicSlots(int count, int linkedIndex)
    {
        StartCoroutine(SpawnDynamicSlotsCoroutine(count, linkedIndex));
    }

    private IEnumerator SpawnDynamicSlotsCoroutine(int count, int linkedIndex)
    {
        var currentEntry = dialogueEntries[currentIndex];

        //find all blank words matching this index in order
        List<WordEntry> matchingWords = new List<WordEntry>();
        foreach (var word in currentEntry.words)
        {
            if (word.isEmpty && word.optionIndices != null)
            {
                foreach (var idx in word.optionIndices)
                {
                    if (idx == linkedIndex)
                    {
                        matchingWords.Add(word);
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            int wordIndex = i + 1;
            int blankIndex = 1 + i;
            
            WordEntry matchingWord = wordIndex < matchingWords.Count ? matchingWords[wordIndex] : matchingWords[matchingWords.Count - 1];

            yield return null;

            currentPlayerMessage.RevealAndGetPosition(blankIndex);

            TextMeshProUGUI blankText = currentPlayerMessage.GetBlankText(blankIndex);

            if (blankText == null)
            {
                continue;
            }


            WordSlots slot = CreateWordSlot(Vector3.zero, matchingWord.options, matchingWord.optionIndices, currentPlayerMessage.GetBlankText(blankIndex));

            slot.isFirstSlot = false;
            activeSlots.Add(slot);
            validator.AddSlot(slot);
        }
    }
    private void SpawnPlayerSentence(DialogueData entry)
    {
        Debug.Log("Bonjour euh spawnPlayerSentence stp goat");
        activeSlots.Clear();

        //Spawn player bubble
        var bubble = Instantiate(playerMessagePrefab, ChatContainer);
        currentPlayerMessage = bubble.GetComponentInChildren<PlayerMessage>();
        
        List<Vector3> blankPositions = currentPlayerMessage.BuildSentence(entry.words, entry.senderName);
        ScrollToBottom();

        if (!entry.words.Exists(w => w.isEmpty))
        {
            currentIndex++;
            Invoke(nameof(ShowNextEntry), 1.5f);
            return;
        }

        //only spawn first blank slot
        WordEntry firstBlank = entry.words.Find(w => w.isEmpty);
        if (firstBlank != null && blankPositions.Count > 0)
        {
            WordSlots slot = CreateWordSlot(Vector3.zero, firstBlank.options, firstBlank.optionIndices, currentPlayerMessage.GetBlankText(0));

            slot.isFirstSlot = true;
            activeSlots.Add(slot);
        }

        //Spawn word cubes for all unique options
        List<string> allOptions = CollectAllOptions(entry);
        SpawnCubes(allOptions);
        
        //setup validator
        validator.SetupSlots(activeSlots);
    }

    //creates a WordSlot GameObject entirely thru code yay
    private WordSlots CreateWordSlot(Vector3 position, string[] options, int[] optionIndices, TMPro.TextMeshProUGUI blankText)
    {
        Debug.Log("Y'a un game object qui se crée normalement");
        
        //create game object
        GameObject slotObject = new GameObject("WordSlot");
        slotObject.transform.position = position;

        //parent to blank text so it moves with it
        if (blankText != null)
        {
            slotObject.transform.SetParent(blankText.transform, false);
            slotObject.transform.localPosition = new Vector3(0, 0, -socketDepthOffset);
        }
        else
        {
            slotObject.transform.position = position;
        }

        //Add XRSocketInteractor
        XRSocketInteractor socket = slotObject.AddComponent<XRSocketInteractor>();
        socket.interactionLayers = wordInteractionLayer;

        //Add a trigger collider for the socket to work
        SphereCollider collider = slotObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.1f;

        //add and initialize WordSlot
        WordSlots slot = slotObject.AddComponent<WordSlots>();
        slot.SetOptions(options, optionIndices);

        //slotText need tmpro ref - pass it after adding component
        slot.slotText = blankText;

        //initialize after all components are added
        slot.Initialize(validator);

        return slot;

    }

    private List<string> CollectAllOptions(DialogueData entry)
    {
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
        return allOptions;
    }

    private void SpawnCubes(List<string> options)
    {
        activeCubes.Clear();

        for (int i = 0; i < options.Count; i++)
        {
            Transform spawnPoint = cubeSpawnPoints[Random.Range(0, cubeSpawnPoints.Length)];
            var cube = Instantiate(wordCubePrefab, spawnPoint.position, spawnPoint.rotation);
            cube.GetComponent<WordCube>().SetWord(options[i]);
            activeCubes.Add(cube);
        }
    }

    public void DestroyAllCubes()
    {
        foreach (var cube in activeCubes)
        {
            if (cube != null)
                Destroy(cube);
        }
        activeCubes.Clear();
    }
    public void OnPlayerTurnComplete(int validatedIndex)
    {
        //clean up active slots
        foreach (var slot in activeSlots)
        {
            if (slot != null)
                //keep text showing before destroying game object
                if (slot.slotText != null)
                    slot.slotText.text = slot.currentWord;
                Destroy(slot.gameObject);
        }
        activeSlots.Clear();

        pendingValidatedIndex = validatedIndex;

        //jump to validated index
        currentIndex = validatedIndex;

        //after playing the validated index, find next non-referenced index
        Invoke(nameof(AdvanceToNextNonReferencedIndex), 0f);
    }

    private void AdvanceToNextNonReferencedIndex()
    {
        ShowNextEntry();
    }
}
