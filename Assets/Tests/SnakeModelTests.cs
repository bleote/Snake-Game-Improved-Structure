using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class SnakeModelTests
{
    private SnakeModel model;
    private DummyLevelGrid dummyLevelGrid;
    private Transform dummyTransform;

    [SetUp]
    public void Setup()
    {
        // Create and initialize DummyGameAssets
        var dummyGameAssets = new GameObject("DummyGameAssets").AddComponent<DummyGameAssets>();
        dummyGameAssets.SnakeBodySprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);

        dummyLevelGrid = new DummyLevelGrid();
        dummyTransform = new GameObject("TestContainer").transform;

        model = new SnakeModel(Vector2Int.zero, 0.1f, SnakeModel.Direction.Right);
        model.Setup(dummyLevelGrid);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(dummyTransform.gameObject);
    }

    [Test]
    public void Snake_Initializes_Correctly()
    {
        Assert.AreEqual(Vector2Int.zero, model.GetGridPosition());
        Assert.AreEqual(SnakeModel.Direction.Right, model.GetCurrentDirection());
        Assert.AreEqual(SnakeModel.State.Alive, model.GetCurrentState());
    }

    [Test]
    public void Snake_Rejects_Backward_Direction()
    {
        model.UpdateDirection(SnakeModel.Direction.Left); // should not change
        Assert.AreEqual(SnakeModel.Direction.Right, model.GetCurrentDirection());
    }

    [Test]
    public void Snake_Updates_Position_After_Tick()
    {
        model.UpdateMovement(0.2f, dummyTransform);
        Assert.AreEqual(new Vector2Int(1, 0), model.GetGridPosition());
    }

    [Test]
    public void Snake_Grows_When_Eating_Food()
    {
        dummyLevelGrid.fakeFoodAt = new Vector2Int(1, 0); // snake will eat this

        model.UpdateMovement(0.2f, dummyTransform); // should grow
        Assert.AreEqual(1, model.GetFullSnakeGridPositionList().Count - 1); // head + 1 body part
    }

    [Test]
    public void Snake_Dies_On_SelfCollision()
    {
        // Feed snake 4 times to be big enough for self-collision
        dummyLevelGrid.fakeFoodAt = new Vector2Int(1, 0);
        model.UpdateMovement(0.2f, dummyTransform); // move right and eat (length = 1)

        dummyLevelGrid.fakeFoodAt = new Vector2Int(2, 0);
        model.UpdateMovement(0.2f, dummyTransform); // move right and eat (length = 2)

        dummyLevelGrid.fakeFoodAt = new Vector2Int(3, 0);
        model.UpdateMovement(0.2f, dummyTransform); // move right and eat (length = 3)

        dummyLevelGrid.fakeFoodAt = new Vector2Int(4, 0);
        model.UpdateMovement(0.2f, dummyTransform); // move right and eat (length = 4)

        // Now start looping
        model.UpdateDirection(SnakeModel.Direction.Down);
        model.UpdateMovement(0.2f, dummyTransform);

        model.UpdateDirection(SnakeModel.Direction.Left);
        model.UpdateMovement(0.2f, dummyTransform);

        model.UpdateDirection(SnakeModel.Direction.Up);
        model.UpdateMovement(0.2f, dummyTransform); // should collide here

        Assert.AreEqual(SnakeModel.State.Dead, model.GetCurrentState());
    }

    public class DummyLevelGrid : LevelGrid
    {
        public Vector2Int fakeFoodAt = new Vector2Int(-1, -1);

        public DummyLevelGrid() : base(20, 20) { }

        public override Vector2Int ValidateGridPosition(Vector2Int pos) => pos;

        public override bool TrySnakeEatFood(Vector2Int pos)
        {
            return pos == fakeFoodAt;
        }
    }

    public class DummyGameAssets : MonoBehaviour
    {
        public Sprite SnakeBodySprite;
        public Sprite FoodSprite;

        private static DummyGameAssets _instance;
        public static DummyGameAssets Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }
    }
}