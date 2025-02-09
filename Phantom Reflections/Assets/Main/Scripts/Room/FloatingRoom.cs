using UnityEngine;

public class FloatingRoom : MonoBehaviour
{
    public float floatRange = 50f;
    public float floatSpeed = 2f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + offset, originalPosition.z);
    }
}
