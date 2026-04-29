using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;

    public void SetData(string name, int score)
    {
        rankText.text = (transform.GetSiblingIndex() + 1).ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
