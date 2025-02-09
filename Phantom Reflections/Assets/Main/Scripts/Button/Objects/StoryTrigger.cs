using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : ButtonBaseFunction
{
    FlowerSystem flowerSys;

    [Space(5)]
    [Header("故事名稱")]
    [SerializeField] private string story;
    //[Header("除靈題目")]
    //[SerializeField] private ExorcismQuestion[] question;

    protected override void Start()
    {
        base.Start();

        flowerSys = FlowerManager.Instance.GetFlowerSystem("TestScene");
        flowerSys.RegisterCommand("SetQuestion", SetQuestionToExorcismGame);
    }

    private void SetQuestionToExorcismGame(List<string> _params)
    {
        //ExorcismGame.Instance.SetQuestions(question);
        //ExorcismGame.Instance.ShowPanel();
    }

    protected override void OnMouseDown()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            flowerSys.ReadTextFromResource(story);
        }

        base.OnMouseDown();
    }
}
