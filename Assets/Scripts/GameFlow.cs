using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class GameFlow : MonoBehaviour
{
    public FixedBlockInitializer initializer;

    [Header("Ordre des niveaux")]
    public List<int> levelOrder = new List<int>() { 0, 1, 2 };

    [Header("Scores par niveau")]
    public List<LevelScoreConfig> levelScores;

    [System.Serializable]
    public class LevelScoreConfig
    {
        public int levelIndex;
        public int scoreYOU;
        public int scoreBeta;
        public int scoreAlpha;
        public int scoreGamma;
    }

    [Header("Debloquer !")]
    public List<GameObject> teleportAreas; // NE PAS mettre la zone de base ici !
    public List<GameObject> UIAnnonce;
    public List<GameObject> Recompenses;

    [Header("Final")]
    public GameObject final;
    public GameObject Thanks;
    public GameObject TxtFinal;
    public GameObject ButtonRestart;
    public float WaitTime = 2f;
    public int prog = 0;
    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    private bool finalLaunched = false;

    private void Start()
    {
        if (initializer == null)
        {
            Debug.LogError("❌ initializer n'est pas assigné dans GameFlow !");
            return;
        }

        initializer.OnMiniGameCompleted += OnLevelCompleted;

        // 🔥 Lancer le premier niveau
        LaunchCurrentLevel();
    }

    private void OnLevelCompleted()
    {
        Debug.Log("Mini-jeu terminé : " + currentLevel);

        // 🔥 ENVOYER LES SCORES AU LEADERBOARD
        SendScoresToLeaderboard(currentLevel);

        // 🔥 Activer la Teleport Area correspondante
        if (currentLevel < teleportAreas.Count && currentLevel < UIAnnonce.Count && currentLevel < Recompenses.Count)
        {
            teleportAreas[currentLevel].SetActive(true);
            UIAnnonce[currentLevel].SetActive(true);
            Recompenses[currentLevel].SetActive(true);
            Debug.Log("Activation Teleport Area : " + teleportAreas[currentLevel].name);
        }

        // 🔥 Passer au niveau suivant
        currentLevel++;

        //if (currentLevel >= levelOrder.Count)
        //{
            //StartCoroutine(LaunchFinal());
        //}

        //if (prog >= 3)
        //{
            //StartCoroutine(LaunchFinal());
        //}
    }

    public void LaunchCurrentLevel()
    {
        if (currentLevel >= levelOrder.Count)
        {
            Debug.Log("Tous les niveaux sont terminés.");
            return;
        }

        int levelIndex = levelOrder[currentLevel];
        Debug.Log("▶ Lancement du niveau : " + levelIndex);

        // 🔥 Lancer le minigame
        initializer.ClearBoard();
        initializer.Initialize(levelIndex);
    }

    public void AddProgress()
    {
        prog++;
        Debug.Log("[GameFlow] Progression : " + prog + " | Instance ID : " + GetInstanceID());

        CheckFinalCondition();
    }

    private void CheckFinalCondition()
    {
        if (!finalLaunched && prog >= 3)
        {
            finalLaunched = true;
            StartCoroutine(LaunchFinal());
        }
    }

    private IEnumerator LaunchFinal()
    {
        Thanks.SetActive(true);
        yield return new WaitForSeconds(WaitTime);

        TxtFinal.SetActive(true);
        final.SetActive(true);
        ButtonRestart.SetActive(true);
    }

    private void SendScoresToLeaderboard(int level)
    {
        var config = levelScores.Find(s => s.levelIndex == level);
        if (config == null)
        {
            Debug.LogWarning("⚠ Aucun score configuré pour le niveau " + level);
            return;
        }

        Dictionary<string, int> scores = new Dictionary<string, int>
        {
            { "YOU", config.scoreYOU },
            { "Beta", config.scoreBeta },
            { "Alpha", config.scoreAlpha },
            { "Gamma", config.scoreGamma }
        };

        LeaderboardManager.Instance.AddScoreForLevel(level, scores);
    }
}
