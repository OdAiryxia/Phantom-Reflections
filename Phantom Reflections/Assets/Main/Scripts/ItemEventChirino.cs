using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEventChirino : MonoBehaviour
{
    [SerializeField] private ExorcismQuestion[] question;
    FlowerSystem flowerSys;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        flowerSys = FlowerManager.Instance.GetFlowerSystem("TestScene");
        flowerSys.RegisterCommand("LockChirino", LockChirino);
        flowerSys.RegisterCommand("ReleaseChirino", ReleaseChirino);
        flowerSys.RegisterCommand("SetQuestion", SetQuestionToExorcismGame);


        flowerSys.RegisterCommand("ProtagonistSprite", (List<string> _params) =>
        {
            if (_params.Count > 0)
            {
                // 嘗試將參數轉換為 int
                if (int.TryParse(_params[0], out int spriteIndex))
                {
                    ProtagonistSprite.instance.ChangeSprite(spriteIndex);
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
    }

    public void chirino()
    {
        flowerSys.SetupDialog();
        flowerSys.ReadTextFromResource("test");

    }

    private void LockChirino(List<string> _params)
    {
        button.enabled = false;
    }

    private void ReleaseChirino(List<string> _params)
    {
        button.enabled = true;
        flowerSys.RemoveDialog();
    }

    private void SetQuestionToExorcismGame(List<string> _params)
    {
        ExorcismGame.Instance.SetQuestions(question);
        ExorcismGame.Instance.ShowPanel();
    }
}
