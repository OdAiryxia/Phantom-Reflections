using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger_2 : MonoBehaviour
{
    FlowerSystem flowerSys;
    [SerializeField] private string 故事;

    private void Start()
    {
        flowerSys = FlowerManager.Instance.GetFlowerSystem("FlowerSystem");
    }

    public void Trigger()
    {
        if (!string.IsNullOrEmpty(故事))
        {
            flowerSys.ReadTextFromResource(故事);
        }
    }
}
