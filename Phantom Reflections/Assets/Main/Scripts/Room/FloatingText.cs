using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float floatRange = 50f;
    public float floatSpeed = 2f;
    public float rotateRange = 10f;
    public float rotateSpeed = 2f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    void Update()
    {
        float positionOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        float rotationOffset = Mathf.Sin(Time.time * rotateSpeed) * rotateRange;

        rectTransform.localPosition = new Vector3(originalPosition.x, originalPosition.y + positionOffset, originalPosition.z);
        rectTransform.localRotation = Quaternion.Euler(0, 0, rotationOffset);
    }
}
