using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public Transform entryContainer; //Objet parent pour les lignes
    public GameObject entryPrefab; //prefab pour une ligne

    private List<LeaderboardFeature> entries = new List<LeaderboardFeature>
    {
        new LeaderboardFeature { playerName = "YOU", score = 9999},
        new LeaderboardFeature { playerName = "Alpha", score = 8500},
        new LeaderboardFeature { playerName = "Beta", score = 7200},
        new LeaderboardFeature { playerName = "Gamma", score = 6100},
    };

    void Start()
    {
        PopulateLeaderboard();
    } 

    void PopulateLeaderboard()
    {
        foreach (var entry in entries)
        {
            var row = Instantiate(entryPrefab, entryContainer);
            row.GetComponent<LeaderboardRow>().SetData(entry.playerName, entry.score);
        }
    }
}
