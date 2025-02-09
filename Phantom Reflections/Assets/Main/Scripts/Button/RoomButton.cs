using UnityEngine;

public class RoomButton : ButtonBaseFunction
{
    [Header("�e���ж��s��")]
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
