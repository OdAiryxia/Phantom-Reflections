using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviour
{
    public static TestSceneManager instance;

    FlowerSystem flowerSys;

    [HideInInspector] public bool buttonInteruption;

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

        flowerSys = FlowerManager.Instance.CreateFlowerSystem("TestScene", true);
        flowerSys.textSpeed = 0.05f;

        buttonInteruption = false;
        DebugTester();
    }

    void Start()
    {
        flowerSys.RegisterCommand("LockButton", LockButton);
        flowerSys.RegisterCommand("ReleaseButton", ReleaseButton);
        flowerSys.RegisterCommand("ProtagonistSprite", (List<string> _params) =>
        {
            if (_params.Count > 0)
            {
                // ���ձN�Ѽ��ഫ�� int
                if (int.TryParse(_params[0], out int spriteIndex))
                {
                    ProtagonistManager.instance.ChangeSprite(spriteIndex);
                }
                else
                {
                    Debug.LogWarning("�L�k�N�Ѽ��ഫ����ơG" + _params[0]);
                }
            }
            else
            {
                Debug.LogWarning("�ʤְѼơI");
            }
        });

        flowerSys.SetupDialog();
        flowerSys.ReadTextFromResource("hide");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) | Input.GetMouseButtonDown(0))
        {
            // Continue the messages, stoping by [w] or [lr] keywords.
            flowerSys.Next();
        }
    }

    void LockButton(List<string> _params)
    {
        buttonInteruption = true;
    }

    void ReleaseButton(List<string> _params)
    {
        buttonInteruption = false;
    }


    [Header("Debug")]
    [SerializeField] bool UI;
    [SerializeField] bool eventSystem;

    void DebugTester()
    {
        if (UI) { SceneManager.LoadSceneAsync((int)SceneIndexes.UI, LoadSceneMode.Additive); }
        if (eventSystem)
        {
            GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>();
        }
    }
}
