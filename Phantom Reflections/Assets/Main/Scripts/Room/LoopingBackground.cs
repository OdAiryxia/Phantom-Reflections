using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 initialPosition;
    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        // 保存起始位置
        initialPosition = transform.position;

        // 計算圖片的一半寬度和高度
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            halfWidth = spriteRenderer.bounds.size.x / 2;
            halfHeight = spriteRenderer.bounds.size.y / 2;
        }
    }

    void Update()
    {
        // 讓物體持續向右下角移動（僅X和Y軸）
        transform.position += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);

        // 當物體移動超出圖片的一半範圍時，將其位置重設回起點
        if (transform.position.x > initialPosition.x + halfWidth || transform.position.x < initialPosition.x - halfWidth ||
            transform.position.y > initialPosition.y + halfHeight || transform.position.y < initialPosition.y - halfHeight)
        {
            transform.position = initialPosition;
        }
    }
}
