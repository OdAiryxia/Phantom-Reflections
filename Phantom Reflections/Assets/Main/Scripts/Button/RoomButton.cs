using UnityEngine;

public class RoomButton : ButtonBaseFunction
{
    [Header("前往房間編號")]
    [SerializeField] private int roomIndex;

    protected override void OnMouseDown()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            RoomManager.instance.MoveToRoom(roomIndex);
        }

        base.OnMouseDown();
    }
}
