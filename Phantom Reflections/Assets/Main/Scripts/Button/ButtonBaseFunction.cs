using System.Collections;
using UnityEngine;
using TMPro;

public class ButtonBaseFunction : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color hoverColor = Color.white;

    private TextMeshPro tagText;
    private Color tagOriginalColor;

    private Coroutine colorCoroutine;
    private Coroutine textCoroutine;
    private float lerpDuration = 0.1f;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        Transform tagTransform = transform.Find("Tag");
        if (tagTransform != null)
        {
            tagText = tagTransform.GetComponent<TextMeshPro>();
            if (tagText != null)
            {
                tagOriginalColor = tagText.color;
                tagText.color = new Color(tagOriginalColor.r, tagOriginalColor.g, tagOriginalColor.b, 0f); // 初始透明
            }
        }
    }

    protected virtual void OnMouseDown()
    {
        if (spriteRenderer != null)
        {
            // 停止之前的 Coroutine（如果有）
            if (colorCoroutine != null)
            {
                StopCoroutine(colorCoroutine);
            }
            colorCoroutine = StartCoroutine(LerpColor(spriteRenderer.color, originalColor));
        }

        if (tagText != null)
        {
            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
            }
            textCoroutine = StartCoroutine(LerpTag(tagText, 0f));
        }
    }

    void OnMouseEnter()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            if (spriteRenderer != null)
            {
                // 停止之前的 Coroutine（如果有）
                if (colorCoroutine != null)
                {
                    StopCoroutine(colorCoroutine);
                }
                colorCoroutine = StartCoroutine(LerpColor(spriteRenderer.color, hoverColor));
            }

            if (tagText != null)
            {
                if (textCoroutine != null)
                {
                    StopCoroutine(textCoroutine);
                }
                textCoroutine = StartCoroutine(LerpTag(tagText, 1f));
            }
        }
    }

    void OnMouseExit()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            if (spriteRenderer != null)
            {
                // 停止之前的 Coroutine（如果有）
                if (colorCoroutine != null)
                {
                    StopCoroutine(colorCoroutine);
                }
                colorCoroutine = StartCoroutine(LerpColor(spriteRenderer.color, originalColor));
            }

            if (tagText != null)
            {
                if (textCoroutine != null)
                {
                    StopCoroutine(textCoroutine);
                }
                textCoroutine = StartCoroutine(LerpTag(tagText, 0f));
            }
        }
    }

    private IEnumerator LerpColor(Color startColor, Color targetColor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        // 確保最後顏色完全到達目標
        spriteRenderer.color = targetColor;
    }

    private IEnumerator LerpTag(TextMeshPro text, float targetAlpha)
    {
        float elapsedTime = 0f;
        float startAlpha = text.color.a;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            text.color = new Color(tagOriginalColor.r, tagOriginalColor.g, tagOriginalColor.b, newAlpha);
            yield return null;
        }

        text.color = new Color(tagOriginalColor.r, tagOriginalColor.g, tagOriginalColor.b, targetAlpha);
    }
}
