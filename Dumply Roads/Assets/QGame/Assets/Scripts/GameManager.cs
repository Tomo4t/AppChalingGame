using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public _UIManager uiManager;
    public int score = 0;
    public TMP_Text scoreText;

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AnswerCorrect()
    {
        score +=10;
        UpdateScoreUI();
    }
    public void AnswerUnCorrect()
    {
        score -= 5;
        UpdateScoreUI();
    }
    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
}
