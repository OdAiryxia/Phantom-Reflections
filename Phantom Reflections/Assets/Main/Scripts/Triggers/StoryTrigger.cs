using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : ButtonBaseFunction
{
    FlowerSystem flowerSys;

    [Space(5)]
    [Header("故事名稱")]
    [SerializeField] private string 故事;
    [SerializeField] private bool 開始觸發故事;

    protected override void Start()
    {
        base.Start();

        flowerSys = FlowerManager.Instance.GetFlowerSystem("FlowerSystem");

        StartCoroutine(TriggerStory());
    }

    IEnumerator TriggerStory()
    {
        yield return null;
        if (開始觸發故事)
        {
            if (!string.IsNullOrEmpty(故事))
            {
                flowerSys.ReadTextFromResource(故事);
            }
            else
            {
                ProgressManager.instance.StartDialogue(ProgressManager.instance.currentChapter);
                ProgressManager.instance.NextChapter();
            }
        }
    }

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            flowerSys.ReadTextFromResource(故事);
        }

        base.OnMouseDown();
    }
}
