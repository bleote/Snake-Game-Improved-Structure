using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LevelGridTests
{
    private TestableLevelGrid levelGrid;
    private DummySnakeController dummySnake;

    [SetUp]
    public void Setup()
    {
        // Create dummy snake
        GameObject dummySnakeGO = new GameObject("DummySnake");
        dummySnake = dummySnakeGO.AddComponent<DummySnakeController>();

        // Use the subclass that skips food spawning
        levelGrid = new TestableLevelGrid(10, 10);
        levelGrid.SetupWithoutFood(dummySnake);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(GameObject.Find("DummySnake"));
    }

    [Test]
    public void ValidateGridPosition_WrapsCorrectly()
    {
        Assert.AreEqual(new Vector2Int(9, 0), levelGrid.ValidateGridPosition(new Vector2Int(-1, 0)));
        Assert.AreEqual(new Vector2Int(0, 0), levelGrid.ValidateGridPosition(new Vector2Int(10, 0)));
        Assert.AreEqual(new Vector2Int(0, 9), levelGrid.ValidateGridPosition(new Vector2Int(0, -1)));
        Assert.AreEqual(new Vector2Int(0, 0), levelGrid.ValidateGridPosition(new Vector2Int(0, 10)));
    }

    // Test food-related logic in PlayMode tests or integration tests

    public class DummySnakeController : SnakeController
    {
        public override List<Vector2Int> GetFullSnakeGridPositionList()
        {
            return new List<Vector2Int>(); // pretend snake is empty
        }
    }

    // Custom subclass that avoids spawning food durinh unit tests
    public class TestableLevelGrid : LevelGrid
    {
        public TestableLevelGrid(int w, int h) : base(w, h) { }

        public void SetupWithoutFood(SnakeController controller)
        {
            // Skip food spawn
            var snakeControllerField = typeof(LevelGrid).GetField("snakeController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            snakeControllerField.SetValue(this, controller);
        }
    }
}
