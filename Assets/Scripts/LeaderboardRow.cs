using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public void SetData(string name, int score)
    {
        //rank is determined by sibling index (0-based, so +1)
        rankText.text = (transform.GetSiblingIndex() + 1).ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
