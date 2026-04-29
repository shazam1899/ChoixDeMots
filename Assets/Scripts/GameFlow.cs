using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    public FixedBlockInitializer initializer;

    [Header("Ordre des niveaux")]
    public List<int> levelOrder = new List<int>() { 0, 1, 2 };

    [Header("Teleport Areas à activer après chaque niveau")]
    public List<GameObject> teleportAreas; // NE PAS mettre la zone de base ici !
    public List<GameObject> allSnapVolumes;
    public List<SnapDisabler> allSnapDisablers;


    [Header("Final")]
    public GameObject final;
    public GameObject Thanks;
    public GameObject TxtFinal;
    public float WaitTime = 2f;

    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    private void Start()
    {
        initializer.OnMiniGameCompleted += OnLevelCompleted;

        // ❌ NE RIEN DÉSACTIVER ICI
        // Tout ce qui est actif dans la scène reste actif
        // Tout ce qui est désactivé reste désactivé

        // 🔥 Lancer le premier niveau
        LaunchCurrentLevel();
    }

    private void OnLevelCompleted()
    {
        Debug.Log("Mini-jeu terminé : " + currentLevel);

        // 🔥 Activer la Teleport Area correspondante
        if (currentLevel < teleportAreas.Count)
        {
            teleportAreas[currentLevel].SetActive(true);
            Debug.Log("Activation Teleport Area  : " + teleportAreas[currentLevel].name);
        }

        // 🔥 Passer au niveau suivant (mais ne pas le lancer)
        currentLevel++;

        if (currentLevel >= levelOrder.Count)
        {
            StartCoroutine(LaunchFinal());
        }
    }

    public void LaunchCurrentLevel()
    {
        if (currentLevel >= levelOrder.Count) return;

        int levelIndex = levelOrder[currentLevel];

        Debug.Log("▶ Lancement du niveau : " + levelIndex);

        // 🔥 Désactiver les SnapDisabler
        foreach (var disabler in allSnapDisablers)
            disabler.enabled = false;

        // 🔥 Réactiver les SnapVolume
        foreach (var snap in allSnapVolumes)
        {
            Debug.Log("Réactivation du SnapVolume : " + snap.name);
            snap.SetActive(true);
        }

        initializer.ClearBoard();
        initializer.Initialize(levelIndex);

        // 🔥 Réactiver SnapDisabler APRÈS la téléportation
        StartCoroutine(ReenableSnapDisablers());
    }

    private IEnumerator ReenableSnapDisablers()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (var disabler in allSnapDisablers)
            disabler.enabled = true;
    }




    private IEnumerator LaunchFinal()
    {
        Thanks.SetActive(true);
        yield return new WaitForSeconds(WaitTime);

        TxtFinal.SetActive(true);
        final.SetActive(true);
    }
}
