using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{

    public static ScoreWindow Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Start() {
        Score.OnHighscoreChanged += Score_OnHighscoreChanged;
        UpdateHighscore();
    }

    private void Score_OnHighscoreChanged(object sender, System.EventArgs e) {
        UpdateHighscore();
    }

    private void Update() {
        scoreText.text = Score.GetScore().ToString();
    }

    private void UpdateHighscore() {
        int highscore = Score.GetHighscore();
        highscoreText.text = "HIGHSCORE\n" + highscore.ToString();
    }

    public void HideScoreWindow() {
        gameObject.SetActive(false);
    }
}
