using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int totalScore;
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [Header("UI")]
    public Transform entryContainer;
    public GameObject entryPrefab;

    // 🔥 LISTE FIXE AVEC LES BONS NOMS
    public List<PlayerScore> players = new List<PlayerScore>();

    private void Awake()
    {
        Instance = this;

        // 🔥 On force un tableau CLEAN à chaque lancement
        players = new List<PlayerScore>
        {
            new PlayerScore { playerName = "TOI", totalScore = 0 },
            new PlayerScore { playerName = "LeGoat404", totalScore = 0 },
            new PlayerScore { playerName = "FouduMoulinRouge", totalScore = 0 },
            new PlayerScore { playerName = "Kauffy", totalScore = 0 },
        };
    }

    private void Start()
    {
        RefreshUI();
    }

    public void AddScoreForLevel(int levelIndex, Dictionary<string, int> levelScores)
    {
        // 🔥 1. Reset des scores
        foreach (var p in players)
            p.totalScore = 0;

        // 🔥 2. Appliquer les scores du niveau
        foreach (var p in players)
        {
            if (levelScores.ContainsKey(p.playerName))
                p.totalScore = levelScores[p.playerName];
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        // 🔥 Nettoyage INSTANTANÉ (évite rank 5,6,7,8)
        for (int i = entryContainer.childCount - 1; i >= 0; i--)
            DestroyImmediate(entryContainer.GetChild(i).gameObject);

        // 🔥 Tri décroissant
        players.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));

        // 🔥 Affichage EXACTEMENT des 4 joueurs
        foreach (var p in players)
        {
            var row = Instantiate(entryPrefab, entryContainer);
            row.GetComponent<LeaderboardRow>().SetData(p.playerName, p.totalScore);
        }
    }
}
