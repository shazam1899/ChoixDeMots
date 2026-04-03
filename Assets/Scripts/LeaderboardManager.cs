using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//coucou : ) ce script a été crée à l'aide d'une IA et de Mr. Lubin 
public class LeaderboardManager : MonoBehaviour
{
    public Transform entryContainer; //Objet parent pour les lignes
    public GameObject entryPrefab; //prefab pour une ligne

    private List<LeaderboardFeature> entries = new List<LeaderboardFeature>
    {
        new LeaderboardFeature { playerName = "YOU", score = 9000},
        new LeaderboardFeature { playerName = "Beta", score = 7200},
        new LeaderboardFeature { playerName = "Alpha", score = 8500},
        new LeaderboardFeature { playerName = "Gamma", score = 6100},
    };

    void Start()
    {
        PopulateLeaderboard();
    } 

    void PopulateLeaderboard()
    {
        foreach (Transform child in entryContainer) //Si le leaderboard est populé, on clear le board
        {
            Destroy(child.gameObject);
        }

        entries.Sort((a, b) => b.score.CompareTo(a.score));

        foreach (var entry in entries)
        {
            var row = Instantiate(entryPrefab, entryContainer);
            row.GetComponent<LeaderboardRow>().SetData(entry.playerName, entry.score);
        }
    }

    public void UpdatePlayerScore(string playerName, int newScore)
    {
        var playerEntry = entries.Find(e => e.playerName == playerName);
        if (playerEntry != null)
        {
            playerEntry.score = newScore;
        }
        else
        {
            entries.Add(new LeaderboardFeature { playerName = playerName, score = newScore });
        }
        PopulateLeaderboard();
    }
}
