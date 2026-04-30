using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Teleport Areas à activer après chaque niveau")]
    public List<GameObject> teleportAreas; // NE PAS mettre la zone de base ici !

    [Header("Final")]
    public GameObject final;
    public GameObject Thanks;
    public GameObject TxtFinal;
    public float WaitTime = 2f;

    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

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
        if (currentLevel < teleportAreas.Count)
        {
            teleportAreas[currentLevel].SetActive(true);
            Debug.Log("Activation Teleport Area : " + teleportAreas[currentLevel].name);
        }

        // 🔥 Passer au niveau suivant
        currentLevel++;

        if (currentLevel >= levelOrder.Count)
        {
            StartCoroutine(LaunchFinal());
        }
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

    private IEnumerator LaunchFinal()
    {
        Thanks.SetActive(true);
        yield return new WaitForSeconds(WaitTime);

        TxtFinal.SetActive(true);
        final.SetActive(true);
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
