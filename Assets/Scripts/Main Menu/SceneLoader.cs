using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu,
    }

    private Action onCompleteCallback;
    private string targetScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(Scene sceneName, Action onComplete = null)
    {
        onCompleteCallback = onComplete;
        targetScene = sceneName.ToString();

        SceneManager.sceneLoaded += OnLoadingSceneLoaded;
        SceneManager.LoadScene(targetScene);
    }

    private void OnLoadingSceneLoaded(UnityEngine.SceneManagement.Scene loadedScene, LoadSceneMode mode)
    {
        if (loadedScene.name != Scene.Loading.ToString()) return;

        SceneManager.sceneLoaded -= OnLoadingSceneLoaded;
        StartCoroutine(LoadTargetSceneAsync());
    }


    private IEnumerator LoadTargetSceneAsync()
    {
        yield return new WaitForSeconds(0.2f); // Optional delay for loading screen to show

        AsyncOperation async = SceneManager.LoadSceneAsync(targetScene);
        while (!async.isDone)
        {
            yield return null;
        }

        onCompleteCallback?.Invoke();
        onCompleteCallback = null;
    }
}
