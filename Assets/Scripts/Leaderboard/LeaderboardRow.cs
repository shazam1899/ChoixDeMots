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

        bool isPlayer = name.Trim().ToUpper() == "TOI";

        Color highlightColor = new Color(1f, 0.84f, 0f); // doré

        if (isPlayer)
        {
            nameText.fontStyle = FontStyles.Bold;
            rankText.fontStyle = FontStyles.Bold;
            scoreText.fontStyle = FontStyles.Bold;

            nameText.color = highlightColor;
            rankText.color = highlightColor;
            scoreText.color = highlightColor;
        }
        else
        {
            nameText.fontStyle = FontStyles.Normal;
            rankText.fontStyle = FontStyles.Normal;
            scoreText.fontStyle = FontStyles.Normal;

            nameText.color = Color.white;
            rankText.color = Color.white;
            scoreText.color = Color.white;
        }
    }
}
