using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class RoomData
{
    public string roomName;             //�ж��W��
    public GameObject roomObject;       //�ж�����A�Ω�s����s
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
    public RoomData[] rooms;                //�ж��ƾڦC��

    private Camera mainCamera;              //�D��v��
    private float cameraLerpSpeed = 5f;     //��v�����ʳt��
    private Vector3 cameraTargetPosition;   //��v���ؼЦ�m

    public TextMeshPro roomNameText;        //�Ω���ܩж��W�٪� UI

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
            Debug.LogWarning("�L�Ī��ж�����");
        }
    }

    //��s�ж��W��
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
