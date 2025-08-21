using CodeMonkey.Utils;
using UnityEngine;

public class PauseWindow : MonoBehaviour
{
    public static PauseWindow Instance { get; private set; }

    [Header("Buttons References")]
    [SerializeField] private Button_UI resumeBtn;
    [SerializeField] private Button_UI mainMenuBtn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetCenteredPauseWindowPosition();

        InitializeButtons();

        HidePauseWindow();
    }

    private void SetCenteredPauseWindowPosition()
    {
        Vector2 zero = Vector2.zero;
        transform.GetComponent<RectTransform>().anchoredPosition = zero;
        transform.GetComponent<RectTransform>().sizeDelta = zero;
    }

    private void InitializeButtons()
    {
        resumeBtn.ClickFunc = () => GameHandler.ResumeGame();
        SoundManager.Instance.SetButtonSounds(resumeBtn);

        mainMenuBtn.ClickFunc = () => SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainMenu);
        SoundManager.Instance.SetButtonSounds(mainMenuBtn);
    }

    public void ShowPauseWindow()
    {
        gameObject.SetActive(true);
    }

    public void HidePauseWindow()
    {
        gameObject.SetActive(false);
    }
}
