using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerEditor : Editor
{
    private DialogueManager manager; 

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

        for (int i = 0; i < manager.dialogueEntries.Count; i++)
        {
            var entry = manager.dialogueEntries[i];

            EditorGUILayout.BeginVertical(GUI.skin.box);

            //Entry header with index and delete button
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Entry {i} - {(entry.isPlayerTurn ? "YOU" : entry.senderName)}", EditorStyles.boldLabel);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                manager.dialogueEntries.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            //Sender and player turn
            entry.senderName = EditorGUILayout.TextField("Sender", entry.senderName);
            entry.isPlayerTurn = EditorGUILayout.Toggle("Is Player Turn", entry.isPlayerTurn);

            if (entry.words == null)
                entry.words = new List<WordEntry>();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Words", EditorStyles.miniBoldLabel);

            for (int w = 0; w < entry.words.Count; w++)
            {
                var wordEntry = entry.words[w];

                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Word {w}", GUILayout.Width(60));

                //is this a blank?
                wordEntry.isEmpty = EditorGUILayout.Toggle("Blank", wordEntry.isEmpty, GUILayout.Width(80));

                if (!wordEntry.isEmpty)
                {
                    //Fixed word
                    wordEntry.word = EditorGUILayout.TextField(wordEntry.word);
                }

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    entry.words.RemoveAt(w);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                //if blank, show options
                if (wordEntry.isEmpty && entry.isPlayerTurn)
                {
                    EditorGUILayout.LabelField("Options", EditorStyles.miniLabel);

                    if (wordEntry.options == null) wordEntry.options = new string [0];
                    if (wordEntry.optionIndices == null) wordEntry.optionIndices = new int[0];

                    int optionCount = EditorGUILayout.IntField("Option count", wordEntry.options.Length);
                    if (optionCount != wordEntry.options.Length)
                    {
                        System.Array.Resize(ref wordEntry.options, optionCount);
                        System.Array.Resize(ref wordEntry.optionIndices, optionCount);
                    }

                    for (int o = 0; o < wordEntry.options.Length; o++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        wordEntry.options[o] = EditorGUILayout.TextField($"Options {o}", wordEntry.options[o]);
                        wordEntry.optionIndices[o] = EditorGUILayout.IntField("→ Index", wordEntry.optionIndices[o], GUILayout.Width(8));
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.EndVertical();
            }

            //Add word button
            if (GUILayout.Button("+ Add Word"))
                entry.words.Add(new WordEntry());

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
            
        }

        //Add entry button
        if (GUILayout.Button("+ Add Dialogue Entry"))
            manager.dialogueEntries.Add(new DialogueData());

        //Mark dirty so changes are saved
        if (GUI.changed)
            EditorUtility.SetDirty(manager);
        
    }
}