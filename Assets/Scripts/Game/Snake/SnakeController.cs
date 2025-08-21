using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private Transform snakeContainer;
    private SnakeModel snakeModel;
    private LevelGrid levelGrid;

    // Remote Config variables
    private float gridMoveTimerMax;
    private SnakeModel.Direction snakeStartingDirection;

    // Input's multi-platform support for different control schemes
    private IGameInput gameInput;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
        gameInput = new PCGameInput();

        LoadSnakeSettingsFromRemoteConfig();

        snakeModel = new SnakeModel(new Vector2Int(10, 10), gridMoveTimerMax, snakeStartingDirection);
        snakeModel.Setup(levelGrid);
    }

    private void LoadSnakeSettingsFromRemoteConfig()
    {
        gridMoveTimerMax = RemoteConfigManager.Instance.GetFloat("gridMoveTimerMax", 0.2f);
        snakeStartingDirection = RemoteConfigManager.Instance.GetEnum("startingDirection", SnakeModel.Direction.Right);
    }

    private void Update()
    {
        switch (snakeModel.GetCurrentState())
        {
            case SnakeModel.State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case SnakeModel.State.Dead:
                break;
        }
    }

    private void HandleInput()
    {
        if (gameInput.UpPressed())
            snakeModel.UpdateDirection(SnakeModel.Direction.Up);

        if (gameInput.DownPressed())
            snakeModel.UpdateDirection(SnakeModel.Direction.Down);

        if (gameInput.LeftPressed())
            snakeModel.UpdateDirection(SnakeModel.Direction.Left);
        
        if (gameInput.RightPressed())
            snakeModel.UpdateDirection(SnakeModel.Direction.Right);
    }

    private void HandleGridMovement()
    {
        bool isAlive = snakeModel.UpdateMovement(Time.deltaTime, snakeContainer);

        if (isAlive)
        {
            // Update visual representation
            Vector2Int gridPosition = snakeModel.GetGridPosition();
            transform.position = new Vector3(gridPosition.x, gridPosition.y);

            Vector2Int directionVector = GetDirectionVector(snakeModel.GetCurrentDirection());
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(directionVector) - 90);

            // Play movement sound
            SoundManager.Instance.PlaySound(SoundManager.Sound.SnakeMove);
        }
        else
        {
            // Snake died
            GameHandler.SnakeDied();
            SoundManager.Instance.PlaySound(SoundManager.Sound.SnakeDie);
        }
    }

    private Vector2Int GetDirectionVector(SnakeModel.Direction direction)
    {
        switch (direction)
        {
            default:
            case SnakeModel.Direction.Right: return new Vector2Int(+1, 0);
            case SnakeModel.Direction.Left: return new Vector2Int(-1, 0);
            case SnakeModel.Direction.Up: return new Vector2Int(0, +1);
            case SnakeModel.Direction.Down: return new Vector2Int(0, -1);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return snakeModel.GetGridPosition();
    }

    public virtual List<Vector2Int> GetFullSnakeGridPositionList()
    {
        return snakeModel.GetFullSnakeGridPositionList();
    }
}