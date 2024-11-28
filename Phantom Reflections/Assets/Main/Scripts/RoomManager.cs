using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RoomData
{
    public GameObject room; // 房間物件
    public string roomName; // 房間名稱
    public Button[] navigationButtons; // 用來導航到其他房間的按鈕
    public int[] targetRoomIndexes; // 每個按鈕對應的目標房間索引
}

public class RoomManager : MonoBehaviour
{
    public RoomData[] rooms; // 房間資料列表
    private int currentRoomIndex = 0; // 當前房間的索引

    public TextMeshProUGUI roomNameText;

    void Start()
    {
        // 初始化顯示當前房間和名稱
        UpdateRoom();
    }

    void UpdateRoom()
    {
        // 隱藏所有房間
        foreach (RoomData roomData in rooms)
        {
            roomData.room.SetActive(false);
        }

        // 顯示當前房間
        rooms[currentRoomIndex].room.SetActive(true);
        roomNameText.text = rooms[currentRoomIndex].roomName;

        // 綁定每個房間內的按鈕事件
        for (int i = 0; i < rooms[currentRoomIndex].navigationButtons.Length; i++)
        {
            int targetIndex = rooms[currentRoomIndex].targetRoomIndexes[i];
            rooms[currentRoomIndex].navigationButtons[i].onClick.RemoveAllListeners();
            rooms[currentRoomIndex].navigationButtons[i].onClick.AddListener(() => GoToRoom(targetIndex));
        }
    }

    void GoToRoom(int targetRoomIndex)
    {
        currentRoomIndex = targetRoomIndex;
        UpdateRoom();
    }
}
