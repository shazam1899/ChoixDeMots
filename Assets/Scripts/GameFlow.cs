using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    public FixedBlockInitializer initializer;

    public List<int> levelOrder = new List<int>() { 0, 1, 2 };

    public GameObject final;
    public GameObject Thanks;
    public GameObject TxtFinal;
    public float WaitTime = 2f;

    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    private void Start()
    {
        initializer.OnMiniGameCompleted += OnLevelCompleted;

        // 🔥 LANCER LE PREMIER NIVEAU AU DÉMARRAGE
        LaunchCurrentLevel();
    }

    private void OnLevelCompleted()
    {
        Debug.Log("Mini-jeu terminé : " + currentLevel);
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

        initializer.ClearBoard();
        initializer.Initialize(levelIndex);
    }

    private IEnumerator LaunchFinal()
    {
        Thanks.SetActive(true);
        yield return new WaitForSeconds(WaitTime);

        TxtFinal.SetActive(true);
        final.SetActive(true);
    }
}
