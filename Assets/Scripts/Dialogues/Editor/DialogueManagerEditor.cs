using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerEditor : Editor
{
    private DialogueManager manager; 
    private List<bool> entryFoldouts = new List<bool>();
    private List<List<bool>> wordFoldouts = new List<List<bool>>();

    private void OnEnable()
    {
        manager = (DialogueManager)target;
    }

    public override void OnInspectorGUI()
    {
        //Draw default fields 
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Dialogue Entries", EditorStyles.boldLabel);

        if (manager.dialogueEntries == null)
            manager.dialogueEntries = new List<DialogueData>();

        //Sync foldout lists
        while (entryFoldouts.Count < manager.dialogueEntries.Count)
            entryFoldouts.Add(true);
        while (wordFoldouts.Count < manager.dialogueEntries.Count)
            wordFoldouts.Add(new List<bool>());

        for (int i = 0; i < manager.dialogueEntries.Count; i++)
        {
            var entry = manager.dialogueEntries[i];
            if (entry == null) continue;

            if (entry.words == null)
                entry.words = new List<WordEntry>();

            //Sync word foldouts
            while (wordFoldouts[i].Count < entry.words.Count)
                wordFoldouts[i].Add(true);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            //Entry header with index and delete button
            EditorGUILayout.BeginHorizontal();
            entryFoldouts[i] = EditorGUILayout.Foldout(entryFoldouts[i], $"Entry {i} - {(entry.isPlayerTurn? "YOU" : entry.senderName)}", true);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                manager.dialogueEntries.RemoveAt(i);
                entryFoldouts.RemoveAt(i);
                wordFoldouts.RemoveAt(i);
                EditorUtility.SetDirty(manager);
                break;
            }
            EditorGUILayout.EndHorizontal();

           if (entryFoldouts[i])
            {
                //Sender and player turn
                entry.senderName = EditorGUILayout.TextField("Sender", entry.senderName);
                entry.isPlayerTurn = EditorGUILayout.Toggle("Is Player Turn", entry.isPlayerTurn);
                entry.isDialogueEnd = EditorGUILayout.Toggle("Is dialogue end?", entry.isDialogueEnd);
                entry.messageAnimation = (GameObject)EditorGUILayout.ObjectField("Message Animation", entry.messageAnimation, typeof(Animator), false);
                entry.bodyAnimation = (GameObject)EditorGUILayout.ObjectField("Body Animation", entry.bodyAnimation, typeof(Animator), false);
                entry.playerBlocked = EditorGUILayout.Toggle("Player blocked", entry.playerBlocked);
                entry.donneReward = EditorGUILayout.Toggle("Give reward?", entry.donneReward);
            
                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField("Words", EditorStyles.miniBoldLabel);

            for (int w = 0; w < entry.words.Count; w++)
            {
                var wordEntry = entry.words[w];
                if (wordEntry == null) continue;

                EditorGUILayout.BeginVertical(GUI.skin.box);

                //Word header with foldout
                EditorGUILayout.BeginHorizontal();
                string wordLabel = wordEntry.isEmpty ? $"Word {w} - [BLANK]" : $"Word {w} - {wordEntry.word}";
                wordFoldouts[i][w] = EditorGUILayout.Foldout(wordFoldouts[i][w], wordLabel, true); 
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    entry.words.RemoveAt(w);
                    wordFoldouts[i].RemoveAt(w);
                    EditorUtility.SetDirty(manager);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                
                if (wordFoldouts[i][w])
                {   
                    //is this a blank?
                    wordEntry.isEmpty = EditorGUILayout.Toggle("Blank", wordEntry.isEmpty);

                    if (!wordEntry.isEmpty)
                    {
                        //Fixed word
                        wordEntry.word = EditorGUILayout.TextField("Word", wordEntry.word);
                    }
                    else if (entry.isPlayerTurn)
                        { 
                            //tick to see if index is needed
                            wordEntry.isIndexed = EditorGUILayout.Toggle("Is indexed", wordEntry.isIndexed);
                            
                            EditorGUILayout.Space(2);
                            EditorGUILayout.LabelField("Options", EditorStyles.miniBoldLabel);

                            //Safe array initialization
                            if (wordEntry.options == null)
                                wordEntry.options = new string[0];
                            if (wordEntry.optionIndices == null)
                                wordEntry.optionIndices = new int[0];

                            //Add/remove option buttons
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"{wordEntry.options.Length} option(s)", GUILayout.Width(80));
                            if (GUILayout.Button("+Add Option", GUILayout.Width(90)))
                            {
                                System.Array.Resize(ref wordEntry.options, wordEntry.options.Length + 1);
                                System.Array.Resize(ref wordEntry.optionIndices, wordEntry.optionIndices.Length + 1);
                                wordEntry.options[wordEntry.options.Length - 1] = "";
                                wordEntry.optionIndices[wordEntry.optionIndices.Length - 1] = 0;
                                EditorUtility.SetDirty(manager);
                            }
                            EditorGUILayout.EndHorizontal();

                            //Draw each option safely
                            for (int o = 0; o < wordEntry.options.Length; o++)
                            {
                                if (o>= wordEntry.optionIndices.Length) break;
                                
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField($" Option {o}", GUILayout.Width(60));
                                wordEntry.options[o] = EditorGUILayout.TextField(wordEntry.options[o] ?? "");
                                EditorGUILayout.LabelField("→ Index", GUILayout.Width(50));
                                wordEntry.optionIndices[o] = EditorGUILayout.IntField(wordEntry.optionIndices[o], GUILayout.Width(40));

                                if (GUILayout.Button("X", GUILayout.Width(20)))
                                {
                                    //Safe removal
                                    var optList = new List<string>(wordEntry.options);
                                    var idxList = new List<int>(wordEntry.optionIndices);
                                    optList.RemoveAt(o);
                                    idxList.RemoveAt(o);
                                    wordEntry.options = optList.ToArray();
                                    wordEntry.optionIndices = idxList.ToArray();
                                    EditorUtility.SetDirty(manager);
                                    break;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                
                //Add word button
                if (GUILayout.Button("+ Add Word"))
                {
                    entry.words.Add(new WordEntry());
                    wordFoldouts[i].Add(true);
                    EditorUtility.SetDirty(manager);
                } 
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);  

        }
        
        //Add entry button
        if (GUILayout.Button("+ Add Dialogue Entry"))
        {
            manager.dialogueEntries.Add(new DialogueData());
            entryFoldouts.Add(true);
            wordFoldouts.Add(new List<bool>());
            EditorUtility.SetDirty(manager);
        }
            
        //Mark dirty so changes are saved
        if (GUI.changed)
            EditorUtility.SetDirty(manager);
      
    }

        
}
