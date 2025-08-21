using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System;

public class GameOverWindow : MonoBehaviour
{

    public static GameOverWindow Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Button_UI retryBtn;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private GameObject newHighscoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        InitializeButtons();

        HideGameOverWindow();
    }

    private void InitializeButtons()
    {
        retryBtn.ClickFunc = () =>
        {
            SceneLoader.Instance.LoadScene(SceneLoader.Scene.GameScene);
        };
        SoundManager.Instance.SetButtonSounds(retryBtn);

        // Add any additional button initializations here if needed
    }

    public void ShowFinalScore(bool isNewHighscore) {
        if (scoreText == null || highscoreText == null)
        {
            Debug.LogError("Score text references are missing!");
            return;
        }

        gameObject.SetActive(true);

        UpdateScoreDisplay(isNewHighscore);
    }

    private void UpdateScoreDisplay(bool isNewHighscore)
    {
        newHighscoreText.SetActive(isNewHighscore);
        scoreText.text = Score.GetScore().ToString();
        highscoreText.text = $"HIGHSCORE {Score.GetHighscore()}";
    }

    private void HideGameOverWindow() {
        gameObject.SetActive(false);
    }
}
