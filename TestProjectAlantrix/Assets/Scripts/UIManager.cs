using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnsText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

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
        HideAllPanels();
        UpdateScore(0);
        UpdateTurns(ScoreManager.Instance.TurnsRemaining);
        UpdateCombo(0);
        if (finalScoreText != null)
            finalScoreText.gameObject.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateTurns(int turnsRemaining)
    {
        if (turnsText != null)
            turnsText.text = "Turns: " + turnsRemaining;
    }

    public void UpdateCombo(int combo)
    {
        if (comboText != null)
        {
            if (combo > 1)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = "Combo x" + combo;
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }

    public void ShowWinPanel()
    {
        HideAllPanels();
        if (winPanel != null) winPanel.SetActive(true);
        ShowFinalScore();
    }

    public void ShowGameOverPanel()
    {
        HideAllPanels();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        ShowFinalScore();
    }

    private void ShowFinalScore()
    {
        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.Score;
        }
    }

    private void HideAllPanels()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (finalScoreText != null) finalScoreText.gameObject.SetActive(false);
    }
}
