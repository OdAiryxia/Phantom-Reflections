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
        // �O�s�_�l��m
        initialPosition = transform.position;

        // �p��Ϥ����@�b�e�שM����
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            halfWidth = spriteRenderer.bounds.size.x / 2;
            halfHeight = spriteRenderer.bounds.size.y / 2;
        }
    }

    void Update()
    {
        // ���������V�k�U�����ʡ]��X�MY�b�^
        transform.position += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);

        // ���鲾�ʶW�X�Ϥ����@�b�d��ɡA�N���m���]�^�_�I
        if (transform.position.x > initialPosition.x + halfWidth || transform.position.x < initialPosition.x - halfWidth ||
            transform.position.y > initialPosition.y + halfHeight || transform.position.y < initialPosition.y - halfHeight)
        {
            transform.position = initialPosition;
        }
    }
}
