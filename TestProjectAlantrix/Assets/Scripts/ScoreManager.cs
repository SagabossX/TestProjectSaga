using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int maxTurns = 20;
    [SerializeField] private int matchPoints = 10;
    [SerializeField] private int mismatchPenalty = 2;
    [SerializeField] private int comboBonus = 5; 

    private int turnsRemaining;
    private int score;
    private int comboCount; 

    public int TurnsRemaining => turnsRemaining;
    public int Score => score;
    public int ComboCount => comboCount;

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
        if (turnsRemaining == 0 && score == 0)
            ResetScore();
        else
            UpdateUI();
    }

    public void ResetScore()
    {
        turnsRemaining = maxTurns;
        score = 0;
        comboCount = 0;
        UpdateUI();
    }

    public void AddMatchPoints()
    {
        comboCount++;

        int totalPoints = matchPoints + (comboCount - 1) * comboBonus;
        score += totalPoints;

        UpdateUI();
    }

    public void AddMismatchPenalty()
    {
        score -= mismatchPenalty;
        if (score < 0) score = 0;

        comboCount = 0; 
        UpdateUI();
    }

    public void UseTurn()
    {
        turnsRemaining--;
        UpdateUI();
    }

    public bool IsOutOfTurns()
    {
        return turnsRemaining <= 0;
    }

    public void LoadState(int savedScore, int savedTurns, int savedCombo = 0)
    {
        score = savedScore;
        turnsRemaining = savedTurns > 0 ? savedTurns : maxTurns;
        comboCount = savedCombo;
        UpdateUI();
    }

    public (int score, int turns, int combo) GetState()
    {
        return (score, turnsRemaining, comboCount);
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateTurns(turnsRemaining);
        UIManager.Instance.UpdateCombo(comboCount);
    }
}
