using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int maxTurns = 20;
    [SerializeField] private int matchPoints = 10;
    [SerializeField] private int mismatchPenalty = 2;

    private int turnsRemaining;
    private int score;

    public int TurnsRemaining => turnsRemaining;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Only reset if no save is loaded
        if (turnsRemaining == 0 && score == 0)
            ResetScore();
    }

    public void ResetScore()
    {
        turnsRemaining = maxTurns;
        score = 0;
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateTurns(turnsRemaining);
    }

    public void AddMatchPoints()
    {
        score += matchPoints;
        UIManager.Instance.UpdateScore(score);
    }

    public void AddMismatchPenalty()
    {
        score -= mismatchPenalty;
        if (score < 0) score = 0;
        UIManager.Instance.UpdateScore(score);
    }

    public void UseTurn()
    {
        turnsRemaining--;
        UIManager.Instance.UpdateTurns(turnsRemaining);
    }

    public bool IsOutOfTurns()
    {
        return turnsRemaining <= 0;
    }

    public void LoadState(int savedScore, int savedTurns)
    {
        score = savedScore;
        turnsRemaining = savedTurns > 0 ? savedTurns : maxTurns; // fallback if old saves
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateTurns(turnsRemaining);
    }
}
