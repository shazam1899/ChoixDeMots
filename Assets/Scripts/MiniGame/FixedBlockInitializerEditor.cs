
//Code réaliser par Dylan LAUNAY avec Copilot

// Script d'éditeur personnalisé pour le composant FixedBlockInitializer, permettant de sélectionner une configuration de niveau dans l'inspecteur et d'initialiser ou réinitialiser le plateau de jeu directement depuis l'éditeur pour faciliter le développement et les tests des niveaux du mini-jeu
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FixedBlockInitializer))]
public class FixedBlockInitializerEditor : Editor
{
    int selectedConfig = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FixedBlockInitializer init = (FixedBlockInitializer)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== Outils de Debug ===", EditorStyles.boldLabel);

        if (init.configurations != null && init.configurations.Count > 0)
        {
            string[] names = new string[init.configurations.Count];
            for (int i = 0; i < names.Length; i++)
                names[i] = string.IsNullOrEmpty(init.configurations[i].name)
                    ? $"Config {i}"
                    : init.configurations[i].name;

            selectedConfig = EditorGUILayout.Popup("Configuration", selectedConfig, names);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Initialize Now"))
                init.Initialize(selectedConfig);

            if (GUILayout.Button("Clear Board"))
                init.ClearBoard();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("Aucune configuration définie.", MessageType.Info);

            if (GUILayout.Button("Clear Board"))
                init.ClearBoard();
        }
    }
}
#endif
