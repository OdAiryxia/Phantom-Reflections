﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogoAnimationLoop : MonoBehaviour
{
    private string folderPath = "Logo"; // 指定PNG序列所在的資料夾路徑 (相對於Resources)
    private float frameRate = 0.05f;
    
    private Sprite[] frames;
    private Image imageComponent;

    private void Start()
    {
        LoadSprites();
        imageComponent = GetComponent<Image>();
        StartCoroutine(PlayAnimation());
    }
    private void LoadSprites()
    {
        frames = Resources.LoadAll<Sprite>(folderPath)
                          .OrderBy(sprite => sprite.name) // 確保按名稱順序排列
                          .ToArray();

        if (frames.Length == 0)
        {
            Debug.LogError("未找到任何Sprite，請檢查路徑或文件名！");
        }
    }

    private IEnumerator PlayAnimation()
    {
        int index = 0;
        while (true)
        {
            imageComponent.sprite = frames[index];
            index++;

            if (index >= frames.Length)
            {
                yield return new WaitForSeconds(10f);
                index = 0;
            }
            else
            {
                yield return new WaitForSeconds(frameRate);
            }
        }
    }
}
