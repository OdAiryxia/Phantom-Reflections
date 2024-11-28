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
