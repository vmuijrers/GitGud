using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public event System.Action<int> OnScoreChanged;

    public int score = 0;
    public int Score
    { 
        get => score;
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }

    public void AddPoint()
    {
        Score++;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}