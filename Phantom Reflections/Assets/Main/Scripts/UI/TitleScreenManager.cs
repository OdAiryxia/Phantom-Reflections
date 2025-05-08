using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button htpButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image vignette;
    [TextArea(20,10)]
    [SerializeField] private string htptext;
    private float alpha;

    void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        htpButton.onClick.AddListener(OnHTPButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);

        alpha = vignette.color.a;

        vignette.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
        Color startColor = vignette.color;
        startColor.a = 1;
        vignette.color = startColor;
    }

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        vignette.rectTransform.position = Vector3.Lerp(vignette.rectTransform.position, mousePosition, Time.deltaTime * 5f);
    }
    private void OnStartButtonClicked()
    {
        ScenesManager.instance.LoadScene();
        PauseManager.instance.Resume();
    }

    private void OnHTPButtonClicked()
    {
        ModalWindowManager.instance.ShowVertical("如何遊玩", null, htptext, "確認", 
            () =>
        {
            ModalWindowManager.instance.Close();
        });
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    IEnumerator FadeIn()
    {
        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            vignette.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one, t);

            Color newColor = vignette.color;
            newColor.a = Mathf.Lerp(1, alpha, t);
            vignette.color = newColor;

            yield return null;
        }

        // 確保最終數值正確
        vignette.transform.localScale = Vector3.one;
        Color finalColor = vignette.color;
        finalColor.a = alpha;
        vignette.color = finalColor;
    }
}
