using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtagonistManager : MonoBehaviour
{
    public static ProtagonistManager instance;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

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

    public void ChangeSprite(int index)
    {
        if (index >= 0 && index < sprites.Length)
        {
            image.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("¯Á¤Ş¶W¥X½d³ò¡I");
        }
    }
}
