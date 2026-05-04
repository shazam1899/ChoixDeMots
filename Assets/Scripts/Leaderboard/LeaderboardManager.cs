//Code réaliser par Dylan LAUNAY à partir du code de base réalise par Tyler GUERIN , avec l'aide de Copilot pour comprendre la logique et debugger
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore // Classe pour stocker le nom du joueur et son score total
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

    public List<PlayerScore> players = new List<PlayerScore>(); // Liste des joueurs et de leurs scores, initialisée dans Awake()

    private void Awake()
    {
        Instance = this;

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
        foreach (var p in players) // Réinitialise les scores totaux avant de les mettre à jour avec les scores du niveau actuel
            p.totalScore = 0;

        foreach (var p in players) // Met à jour les scores totaux des joueurs en fonction des scores du niveau actuel
        {
            if (levelScores.ContainsKey(p.playerName))
                p.totalScore = levelScores[p.playerName];
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = entryContainer.childCount - 1; i >= 0; i--) // Supprime tous les éléments enfants de entryContainer pour rafraîchir l'affichage du leaderboard
            DestroyImmediate(entryContainer.GetChild(i).gameObject);

        players.Sort((a, b) => b.totalScore.CompareTo(a.totalScore)); // Trie les joueurs par score total décroissant pour afficher le meilleur score en haut du leaderboard

        foreach (var p in players)
        {
            var row = Instantiate(entryPrefab, entryContainer); // Instancie une nouvelle ligne de leaderboard à partir du prefab entryPrefab et la place sous entryContainer
            row.GetComponent<LeaderboardRow>().SetData(p.playerName, p.totalScore); // Appelle la méthode SetData de LeaderboardRow pour afficher le nom du joueur et son score total dans la ligne du leaderboard
        }
    }
}
