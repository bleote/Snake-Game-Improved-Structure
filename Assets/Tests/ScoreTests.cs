using NUnit.Framework;
using UnityEngine;
using System;

public class ScoreTests
{
    [SetUp]
    public void Setup()
    {
        PlayerPrefs.DeleteAll(); // Clear highscore
        Score.InitializeStatic();
    }

    [Test]
    public void InitializeStatic_ResetsScoreToZero()
    {
        Score.AddScore();
        Score.InitializeStatic();
        Assert.AreEqual(0, Score.GetScore());
    }

    [Test]
    public void AddScore_IncrementsScoreBy100()
    {
        Score.AddScore();
        Assert.AreEqual(100, Score.GetScore());

        Score.AddScore();
        Assert.AreEqual(200, Score.GetScore());
    }

    [Test]
    public void TrySetNewHighscore_OnlyUpdatesWhenHigher()
    {
        // Set initial highscore
        PlayerPrefs.SetInt("highscore", 150);

        // Lower score shouldn't update
        bool resultLow = Score.TrySetNewHighscore(100);
        Assert.IsFalse(resultLow);
        Assert.AreEqual(150, PlayerPrefs.GetInt("highscore"));

        // Higher score should update
        bool resultHigh = Score.TrySetNewHighscore(200);
        Assert.IsTrue(resultHigh);
        Assert.AreEqual(200, PlayerPrefs.GetInt("highscore"));
    }

    [Test]
    public void TrySetNewHighscore_UsesCurrentScore_WhenNoArgPassed()
    {
        Score.AddScore(); // 100
        Score.AddScore(); // 200
        Score.TrySetNewHighscore(); // should set 200
        Assert.AreEqual(200, PlayerPrefs.GetInt("highscore"));
    }

    [Test]
    public void OnHighscoreChanged_IsFiredWhenNewHighscoreSet()
    {
        PlayerPrefs.SetInt("highscore", 100);
        bool eventFired = false;

        Score.OnHighscoreChanged += (_, _) => eventFired = true;
        Score.TrySetNewHighscore(150);

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnHighscoreChanged_IsNotFiredWhenScoreNotBeaten()
    {
        PlayerPrefs.SetInt("highscore", 200);
        bool eventFired = false;

        Score.OnHighscoreChanged += (_, _) => eventFired = true;
        Score.TrySetNewHighscore(150); // won't beat highscore

        Assert.IsFalse(eventFired);
    }
}
