using UnityEngine;

public class PCGameInput : IGameInput
{
    public bool PausePressed() => Input.GetKeyDown(KeyCode.Escape);

    public bool UpPressed() => Input.GetKeyDown(KeyCode.UpArrow);
    public bool DownPressed() => Input.GetKeyDown(KeyCode.DownArrow);
    public bool LeftPressed() => Input.GetKeyDown(KeyCode.LeftArrow);
    public bool RightPressed() => Input.GetKeyDown(KeyCode.RightArrow);
}