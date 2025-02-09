using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class RoomData
{
    public string roomName;             //房間名稱
    public GameObject roomObject;       //房間物件，用於存放按鈕
    public Vector2 position
    {
        get
        {
            return roomObject != null ? (Vector2)roomObject.transform.position : Vector2.zero;
        }
    }
}

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public RoomData[] rooms;                //房間數據列表

    private Camera mainCamera;              //主攝影機
    private float cameraLerpSpeed = 5f;     //攝影機移動速度
    private Vector3 cameraTargetPosition;   //攝影機目標位置

    public TextMeshPro roomNameText;        //用於顯示房間名稱的 UI

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        mainCamera = Camera.main;

        if (rooms != null && rooms.Length > 1)
        {
            cameraTargetPosition = new Vector3(rooms[1].position.x, rooms[1].position.y, mainCamera.transform.position.z);
        }
        UpdateRoomNameUI(1);
    }

    void Update()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTargetPosition, Time.deltaTime * cameraLerpSpeed);
    }

    public void MoveToRoom(int roomIndex)
    {
        if (roomIndex >= 0 && roomIndex < rooms.Length)
        {
            RoomData targetRoom = rooms[roomIndex];
            cameraTargetPosition = new Vector3(targetRoom.position.x, targetRoom.position.y, mainCamera.transform.position.z);

            UpdateRoomNameUI(roomIndex);
        }
        else
        {
            Debug.LogWarning("無效的房間索引");
        }
    }

    //更新房間名稱
    void UpdateRoomNameUI(int roomIndex)
    {
        if (roomIndex >= 0 && roomIndex < rooms.Length)
        {
            roomNameText.text = rooms[roomIndex].roomName;
        }
        else
        {
            roomNameText.text = "Unknown Room";
        }
    }
}
