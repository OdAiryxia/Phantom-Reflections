using UnityEngine;
using UnityEditor;

public class RoomButton : ButtonBaseFunction
{
    [Header("前往房間編號")]
    [SerializeField] private int roomIndex;

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            RoomManager.instance.MoveToRoom(roomIndex);
        }

        base.OnMouseDown();
    }

#if UNITY_EDITOR
    [System.Obsolete]
    private void OnDrawGizmosSelected()
{
    // 嘗試從場景中取得 RoomManager
    RoomManager rm = Application.isPlaying ? RoomManager.instance : FindObjectOfType<RoomManager>();
    
    if (rm == null || rm.rooms == null)
        return;

    // 確保索引有效
    if (roomIndex >= 0 && roomIndex < rm.rooms.Length)
    {
        RoomData targetRoom = rm.rooms[roomIndex];
        if (targetRoom.roomObject != null)
        {
            // 畫線
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
            Gizmos.DrawLine(transform.position, targetRoom.roomObject.transform.position);

            // 顯示房間名稱
            Vector3 labelPos = targetRoom.roomObject.transform.position + Vector3.up * 0.5f;
            Handles.Label(labelPos, $"Room {roomIndex}：{targetRoom.roomName}");
        }
    }
    else
    {
        Handles.Label(transform.position + Vector3.up * 0.5f, $"無效的房間 Index：{roomIndex}");
    }
}
#endif
}
