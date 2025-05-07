using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Flower.FlowerSystem;

public class ProgressManager : MonoBehaviour
{
    private FlowerSystem flowerSys;
    public static ProgressManager instance;

    public AudioSource audioSource;

    [SerializeField] private GameObject case_2_object;
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
        flowerSys = FlowerManager.Instance.CreateFlowerSystem("FlowerSystem", true);
    }

    void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

        flowerSys.textSpeed = 0.05f;
        flowerSys.SetScreenReference(Screen.currentResolution.width, Screen.currentResolution.height);
        flowerSys.RegisterCommand("LockButton", LockButton);
        flowerSys.RegisterCommand("ReleaseButton", ReleaseButton);
        flowerSys.RegisterCommand("SetQuestions", SetQuestions);
        flowerSys.RegisterCommand("NextChapter", NextCpt);
        flowerSys.RegisterCommand("AudioPlay", AudioPlay);
        flowerSys.RegisterCommand("AudioStop", AudioStop);
        flowerSys.RegisterCommand("ProtagonistSprite", (List<string> _params) =>
        {
            if (_params.Count > 0)
            {
                // 嘗試將參數轉換為 int
                if (int.TryParse(_params[0], out int spriteIndex))
                {
                    ProtagonistManager.instance.ChangeSprite(spriteIndex);
                }
                else
                {
                    Debug.LogWarning("無法將參數轉換為整數：" + _params[0]);
                }
            }
            else
            {
                Debug.LogWarning("缺少參數！");
            }
        });
        flowerSys.SetupDialog();
        flowerSys.SetupUIStage();
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

    #region Commands
    void LockButton(List<string> _params)
    {
        buttonInteruption = true;

        if (CGManager.instance.onCg)
        {
            CGManager.instance.closeButton.gameObject.SetActive(false);

            foreach (Button button in CGManager.instance.cgButtons)
            {
                button.enabled = false;
            }
        }
    }

    void ReleaseButton(List<string> _params)
    {
        buttonInteruption = false;

        if (CGManager.instance.onCg)
        {
            CGManager.instance.closeButton.gameObject.SetActive(true);

            foreach (Button button in CGManager.instance.cgButtons)
            {
                button.enabled = true;
            }
        }
    }

    void NextCpt(List<string> _params)
    {
        StartDialogue(currentChapter);
        currentChapter++;
    }

    [HideInInspector] public bool setQuestionsAfterStory;
    void SetQuestions(List<string> _params)
    {
        if (setQuestionsAfterStory)
        {
            ExorcismManager.instance.SetQuestion();
            setQuestionsAfterStory = false;
        }
        else
        {
            return;
        }
    }

    void AudioPlay(List<string> _params)
    {
        audioSource.Play();
    }

    void AudioStop(List<string> _params)
    {
        audioSource.Stop();
    }

    #endregion

    public int currentChapter = 0;
    public bool buttonInteruption = false;

    public void StartDialogue(int chapter)
    {
        switch (chapter)
        {
            case 0:
                flowerSys.ReadTextFromResource("前劇");
                break;
            case 1:
                break;
            case 2:
                flowerSys.ReadTextFromResource("test_1");
                case_2_object.SetActive(false);
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    public void NextChapter()
    {
        currentChapter++;
    }
}
