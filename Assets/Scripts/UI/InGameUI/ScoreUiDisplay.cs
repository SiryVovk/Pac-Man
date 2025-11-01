using TMPro;
using UnityEngine;

public class ScoreUiDisplay : MonoBehaviour
{
    [SerializeField] private Score score;
    [SerializeField] private TMP_Text scoreDisplayPlace;

    private void OnEnable()
    {
        score.OnScoreChange += DisplayScorText;
    }

    private void OnDisable()
    {
        score.OnScoreChange -= DisplayScorText;
    }

    private void DisplayScorText(int points)
    {
        scoreDisplayPlace.text = points.ToString("D4");
    }
}
