
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Settings")]
    public int score;
    public TMP_Text scoreText;
    [SerializeField] private HoleMovement holeMovement;
    [SerializeField] private int[] levelThresholds = { 0,10, 30, 60, 100 };
    private int _currentLevel;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        score = 0;
        UpdateScoreUI();
        UpdateHoleLevelByScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        UpdateHoleLevelByScore();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    void UpdateHoleLevelByScore()
    {
        for (int i = levelThresholds.Length - 1; i >= 0; i--)
        {
            if (score >= levelThresholds[i])
            {
                if (i > _currentLevel)
                {
                    _currentLevel = i;
                    holeMovement.SetHoleLevel(_currentLevel);
                }
                break;
            }
        }
    }
    
    
}

