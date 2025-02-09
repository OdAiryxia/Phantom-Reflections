using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProtagonistAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image imageComponent;

    [SerializeField] private float normalAlpha = 1.0f;
    [SerializeField] private float hoverAlpha = 0.5f;
    [SerializeField] private float fadeDuration = 0.3f;

    private Coroutine fadeCoroutine;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(hoverAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(normalAlpha);
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = imageComponent.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (imageComponent != null)
        {
            Color color = imageComponent.color;
            color.a = alpha;
            imageComponent.color = color;
        }
    }
}
