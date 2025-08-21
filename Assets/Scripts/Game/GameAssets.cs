using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance { get; private set; }

    public Sprite SnakeHeadSprite { get; private set; }
    public Sprite SnakeBodySprite { get; private set; }
    public Sprite FoodSprite { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAssetsFromRemoteConfig();
    }

    private void LoadAssetsFromRemoteConfig()
    {
        SnakeHeadSprite = LoadSprite(RemoteConfigManager.Instance.GetString("snakeHeadSprite", "SnakeHead"));
        SnakeBodySprite = LoadSprite(RemoteConfigManager.Instance.GetString("snakeBodySprite", "SnakeBody"));
        FoodSprite = LoadSprite(RemoteConfigManager.Instance.GetString("foodSprite", "FoodApple"));
    }

    private Sprite LoadSprite(string resourcePath)
    {
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite == null)
        {
            Debug.LogError($"Failed to load sprite from path: {resourcePath}");
        }
        return sprite;
    }
}
