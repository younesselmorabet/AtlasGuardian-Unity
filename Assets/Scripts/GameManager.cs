using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Durée du jeu")]
    public float gameDuration = 180f; // 3 minutes pour survivre

    [Header("Score")]
    public int score = 0;

    [Header("Villages (glisser depuis la scène)")]
    public List<Village> allVillages = new List<Village>();

    [Header("UI")]
    public Text scoreText;
    public Text timerText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private float timeRemaining;
    private bool gameActive = true;

    void Awake()
    {
        instance = this;
        timeRemaining = gameDuration;
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;

        // Afficher le timer
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        if (timerText) timerText.text = $"{minutes:00}:{seconds:00}";

        if (timeRemaining <= 0) Victory();
    }

    public void AddScore(int points)
    {
        score += points;
        if (scoreText) scoreText.text = "Score: " + score;
    }

    public void GameOver(string villageName)
    {
        if (!gameActive) return;
        gameActive = false;
        Time.timeScale = 0f;
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Debug.Log("GAME OVER — Village effondré: " + villageName);
    }

    public void Victory()
    {
        if (!gameActive) return;
        gameActive = false;
        Time.timeScale = 0f;
        if (victoryPanel) victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Utilisé par l'IA pour trouver le village le plus en danger
    public Village GetMostCriticalVillage()
    {
        Village critical = null;
        float lowestStat = float.MaxValue;

        foreach (Village v in allVillages)
        {
            float minStat = Mathf.Min(v.water, v.health, v.food);
            if (minStat < lowestStat)
            {
                lowestStat = minStat;
                critical = v;
            }
        }
        return critical;
    }
}