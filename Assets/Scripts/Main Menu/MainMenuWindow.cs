using CodeMonkey.Utils;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    public static MainMenuWindow Instance { get; private set; }
    
    private enum Sub
    {
        Main,
        HowToPlay,
    }

    [Header("SubMenus")]
    [SerializeField] private GameObject mainSub;
    [SerializeField] private GameObject howToPlaySub;

    [Header("Buttons - Main")]
    [SerializeField] private Button_UI playBtn;
    [SerializeField] private Button_UI howToPlayBtn;
    [SerializeField] private Button_UI quitBtn;

    [Header("Buttons - How To Play")]
    [SerializeField] private Button_UI backBtn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetCenteredMenuPositions();

        InitializeButtons();

        // Start with main menu
        ShowSub(Sub.Main);
    }

    private void SetCenteredMenuPositions()
    {
        Vector2 zero = Vector2.zero;
        mainSub.GetComponent<RectTransform>().anchoredPosition = zero;
        howToPlaySub.GetComponent<RectTransform>().anchoredPosition = zero;
    }

    private void InitializeButtons()
    {
        playBtn.ClickFunc = () => SceneLoader.Instance.LoadScene(SceneLoader.Scene.GameScene);
        SoundManager.Instance.SetButtonSounds(playBtn);
        
        howToPlayBtn.ClickFunc = () => ShowSub(Sub.HowToPlay);
        SoundManager.Instance.SetButtonSounds(howToPlayBtn);
        
        quitBtn.ClickFunc = () => Application.Quit();
        SoundManager.Instance.SetButtonSounds(quitBtn);
        
        backBtn.ClickFunc = () => ShowSub(Sub.Main);
        SoundManager.Instance.SetButtonSounds(backBtn);
    }

    private void ShowSub(Sub sub)
    {
        mainSub.SetActive(false);
        howToPlaySub.SetActive(false);

        switch (sub)
        {
            case Sub.Main:
                mainSub.SetActive(true);
                break;

            case Sub.HowToPlay:
                howToPlaySub.SetActive(true);
                break;
        }
    }
}
