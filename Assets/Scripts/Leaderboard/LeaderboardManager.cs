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

    [Header("Players")]
    public List<PlayerScore> players = new List<PlayerScore>
    {
        new PlayerScore { playerName = "YOU", totalScore = 0 },
        new PlayerScore { playerName = "Beta", totalScore = 0 },
        new PlayerScore { playerName = "Alpha", totalScore = 0 },
        new PlayerScore { playerName = "Gamma", totalScore = 0 },
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshUI();
    }

    public void AddScoreForLevel(int levelIndex, Dictionary<string, int> levelScores)
    {
        foreach (var kvp in levelScores)
        {
            var player = players.Find(p => p.playerName == kvp.Key);
            if (player != null)
                player.totalScore += kvp.Value;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in entryContainer)
            Destroy(child.gameObject);

        players.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));

        foreach (var p in players)
        {
            var row = Instantiate(entryPrefab, entryContainer);
            row.GetComponent<LeaderboardRow>().SetData(p.playerName, p.totalScore);
        }
    }

    public void AddScoreForLevel(int levelIndex, Dictionary<string, int> levelScores, string npcName = "")
    {
        foreach (var kvp in levelScores)
        {
            var player = players.Find(p => p.playerName == kvp.Key);
            if (player != null)
                player.totalScore += kvp.Value;
        }

        RefreshUI();

        //trigger friend notification
        if (FriendNotificationManager.Instance != null && npcName != "")
            FriendNotificationManager.Instance.TriggerFriendNotification(npcName);
    }
}
