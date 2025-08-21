using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a mock implementation of a Remote Config system that I like to use.
/// In production, this would pull JSON from Firebase, PlayFab, or a custom backend.
/// </summary>
public class RemoteConfigManager : MonoBehaviour
{
    public static RemoteConfigManager Instance { get; private set; }

    private RemoteConfigData configData;
    private Dictionary<string, string> configValues;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadRemoteConfig();
    }

    private void LoadRemoteConfig()
    {
        // In production, we must replace this with fetched JSON from the remote backend
        TextAsset json = Resources.Load<TextAsset>("Remote Config/mock_remote_config");

        if (json != null)
        {
            configData = JsonUtility.FromJson<RemoteConfigData>(json.text);
            ConvertToDictionary(configData);
        }
        else
        {
            Debug.LogWarning("RemoteConfig JSON not found. Using fallback defaults.");
            configValues = new Dictionary<string, string>(); // prevent null reference
        }
    }

    private void ConvertToDictionary(RemoteConfigData data)
    {
        configValues = new Dictionary<string, string>
        {
            { "snakeHeadSprite", data.snakeHeadSprite },
            { "snakeBodySprite", data.snakeBodySprite },
            { "foodSprite", data.foodSprite },
            { "gridMoveTimerMax", data.gridMoveTimerMax.ToString() },
            { "startingDirection", data.startingDirection.ToString() }
        };
    }

    public string GetString(string key, string defaultValue = "") =>
        configValues.TryGetValue(key, out var value) ? value : defaultValue;

    public float GetFloat(string key, float defaultValue = 0f) =>
        float.TryParse(GetString(key), out var result) ? result : defaultValue;

    public TEnum GetEnum<TEnum>(string key, TEnum defaultValue) where TEnum : struct =>
        Enum.TryParse(GetString(key), out TEnum result) ? result : defaultValue;
}

[Serializable]
public class RemoteConfigData
{
    public string snakeHeadSprite = "SnakeHead";
    public string snakeBodySprite = "SnakeBody";
    public string foodSprite = "FoodApple";
    public float gridMoveTimerMax = 0.2f;
    public Direction startingDirection = Direction.Right;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
