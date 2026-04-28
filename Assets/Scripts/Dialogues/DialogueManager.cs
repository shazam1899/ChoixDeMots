using System.Collections.Generic;
using System.Collections;
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
    private AnimationClip currentAnimation;
    public Animation animationVisage;

    //feature "se faire bloquer"
    public Bloquer blockController;
    //public GameObject blockedAnimation;
    private bool blockedTriggered = false;
    

    void Start()
    {
        validator = FindFirstObjectByType<SentenceValidator>();
        ShowNextEntry();

        if (blockController == null)
            blockController = FindFirstObjectByType<Bloquer>();
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
        
        if (entry.playerBlocked)
        {
            HandlePlayerBlocked();
            return;
        }

        if (!entry.isPlayerTurn)
        {
            //Spawn NPC Message
            string message = BuildSentence(entry.words);
            string messageVisible = BuildSentence(entry.words);
            var bubble = Instantiate(npcMessagePrefab, ChatContainer);
            bubble.GetComponentInChildren<NPCMessage>().SetMessage(entry.senderName, message, messageVisible);
            ScrollToBottom();
            currentIndex++;

            //Play animation if assigned
            if (entry.messageAnimation != null && entry.messageAnimation != currentAnimation)
            {
                PlayMessageAnimation(entry.messageAnimation);
                currentAnimation = entry.messageAnimation; //store as current
            }
            //if no animation is assigned, previous one keeps playing 

            //stop dialogue if entry is marked as the end
            if (entry.isDialogueEnd)
            {
                Debug.Log("dialogue ended!");
                return;
            }
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
            sentence += word.isEmpty ? "[...]" : word.word + " ";
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
        List<WordEntry> matchingIndexedWords = new List<WordEntry>();
        foreach (var word in currentEntry.words)
        {
            if (word.isEmpty && word.optionIndices != null)
            {
                foreach (var idx in word.optionIndices)
                {
                    if (idx == linkedIndex)
                    {
                        matchingIndexedWords.Add(word);
                        break;
                    }
                }
            }
        }
        //collect all blank indices with their word entries in order
        List<(int blankIndex, WordEntry word)> blanksToSpawn = new List<(int, WordEntry)>();

        int blankIdx = 0;
        foreach (var word in currentEntry.words)
        {
            if (word.isEmpty)
            {
                if (blankIdx > 0) //skip first slot already spawned
                {
                    if (word.isIndexed)
                    {
                        //only add if it matches linkedIndex
                        bool matchesIndex = false;
                        if (word.optionIndices != null)
                        {
                            foreach (var idx in word.optionIndices)
                            {
                                if (idx == linkedIndex)
                                {
                                    matchesIndex = true;
                                    break;
                                }
                            }
                        }
                        if (matchesIndex)
                            blanksToSpawn.Add((blankIdx, word));
                    }
                    else
                    {
                        //always add non indexed slots
                        blanksToSpawn.Add((blankIdx, word));
                    }
                }
                blankIdx++;
            }
        }

        //spawn all needed slots in order
        foreach (var (targetBlankIndex, wordEntry) in blanksToSpawn)
        {
            yield return null;

            currentPlayerMessage.RevealBlank(targetBlankIndex);

            yield return null;

            Canvas rootCanvas = currentPlayerMessage.GetComponentInParent<Canvas>();
            if (rootCanvas != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rootCanvas.GetComponent<RectTransform>());

            Canvas.ForceUpdateCanvases();
            yield return null;
            
            TextMeshProUGUI blankText = currentPlayerMessage.GetBlankText(targetBlankIndex);

            if (blankText == null)
            {
                continue;
            }
        
            WordSlots slot = CreateWordSlot(Vector3.zero, wordEntry.options, wordEntry.optionIndices, blankText);

            slot.isFirstSlot = false;
            slot.isIndexed = wordEntry.isIndexed; //pass isIndexed
            activeSlots.Add(slot);
            validator.AddSlot(slot);
        }
    }
    private void SpawnPlayerSentence(DialogueData entry)
    {
        activeSlots.Clear();

        //Spawn player bubble
        var bubble = Instantiate(playerMessagePrefab, ChatContainer);
        currentPlayerMessage = bubble.GetComponentInChildren<PlayerMessage>();
        
        List<Vector3> blankPositions = currentPlayerMessage.BuildSentence(entry.words, entry.senderName);

        //prebuild all hidden sentence variants
        currentPlayerMessage.BuildSentenceVariants(entry.words);

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
            slot.isIndexed = firstBlank.isIndexed; //pass isIndexed
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
        PlayerMessage bubbleToUpdate = currentPlayerMessage;
        
        //clean up active slots
        foreach (var slot in activeSlots)
        {
            if (slot != null)
            {    //keep text showing before destroying game object
                slot.transform.SetParent(null);
                Destroy(slot.gameObject);
            }
        }
        activeSlots.Clear();

        bubbleToUpdate.ShowValidatedSentence(validatedIndex);

        pendingValidatedIndex = validatedIndex;

        var currentEntry = dialogueEntries[currentIndex];

        if (currentEntry.playerBlocked)
        {
            HandlePlayerBlocked();
            return;
        }

        //jump to validated index
        currentIndex = validatedIndex;

        //after playing the validated index, find next non-referenced index
        Invoke(nameof(AdvanceToNextNonReferencedIndex), 2f);
    }

    private void AdvanceToNextNonReferencedIndex()
    {
        ShowNextEntry();
    }

    /// <summary>
    /// Plays an animation clip on the animator if available
    /// </summary>
    /// <param name="animationClip">The animation clip to play</param>
    public void PlayMessageAnimation(AnimationClip animationClip)
    {
        if (animationVisage == null)
        {
            Debug.LogWarning("AnimationVisage component not assigned on DialogueManager!");
            return;
        }
        // Add the animation to the Animation component and play it
        animationVisage.AddClip(animationClip, animationClip.name);
        animationVisage.Play(animationClip.name);
    }

    private void HandlePlayerBlocked()
    {
        if (blockedTriggered)
            return;

        blockedTriggered = true;

        DestroyAllCubes();
        // clear else uhh ajoute qq chose estuplé

        //if (blockedAnimation != null && blockedAnimation != currentAnimation)
        //{
            //PlayMessageAnimation(blockedAnimation);
            //currentAnimation = blockedAnimation;
        //}

        if (blockController != null)
            blockController.LanceEffect();
        else
            Debug.Log("DialogueManager needs a bloquer reference for blocked behaviour.");
    }
}
