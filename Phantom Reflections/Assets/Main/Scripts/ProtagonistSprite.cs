using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtagonistSprite : MonoBehaviour
{

    public static ProtagonistSprite instance;
    [SerializeField] private Image imageComponent;
    [SerializeField] private Sprite[] sprites;

    private void Awake()
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

    /// <summary>
    /// 根據傳入的索引更換 Sprite
    /// </summary>
    /// <param name="index">對應 Sprite 陣列的索引</param>
    public void ChangeSprite(int index)
    {
        if (index >= 0 && index < sprites.Length)
        {
            imageComponent.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("索引超出範圍！");
        }
    }
}
