//Code réaliser par Tyler GUERIN, Modifier par Dylan LAUNAY pour ajuster au nouveau système de score
using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;

    public void SetData(string name, int score) // Méthode pour configurer les données d'une ligne du leaderboard avec le nom du joueur et son score total
    {
        rankText.text = (transform.GetSiblingIndex() + 1).ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
