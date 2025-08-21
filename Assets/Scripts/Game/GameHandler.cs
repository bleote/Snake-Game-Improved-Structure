using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    [SerializeField] private SnakeController snakeController;

    private LevelGrid levelGrid;

    // Input's multi-platform support for different control schemes
    private IGameInput gameInput;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameInput = new PCGameInput();

        Score.InitializeStatic();
        Time.timeScale = 1f;
    }

    private void Start()
    {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        snakeController.Setup(levelGrid);
        levelGrid.Setup(snakeController);
    }

    private void Update()
    {
        if (gameInput.PausePressed())
        {
            if (IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public static void SnakeDied() {
        bool isNewHighscore = Score.TrySetNewHighscore();
        GameOverWindow.Instance.ShowFinalScore(isNewHighscore);
        ScoreWindow.Instance.HideScoreWindow();
    }

    public static void ResumeGame() {
        PauseWindow.Instance.HidePauseWindow();
        Time.timeScale = 1f;
    }

    public static void PauseGame() {
        PauseWindow.Instance.ShowPauseWindow();
        Time.timeScale = 0f;
    }

    public static bool IsGamePaused() {
        return Time.timeScale == 0f;
    }
}
