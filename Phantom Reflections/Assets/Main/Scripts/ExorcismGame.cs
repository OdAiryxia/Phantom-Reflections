using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExorcismGame : MonoBehaviour
{
    public static ExorcismGame Instance;

    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    public Image[] lifeIcons;

    public TextMeshProUGUI timerText;
    public CanvasGroup panelCanvasGroup;

    [SerializeField] private ExorcismQuestion[] questions;
    private int currentQuestionIndex = 0;
    private int lives = 3;
    public Slider slider;
    private float timeRemaining = 30f;
    private bool gameEnded = false;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;
        UpdateLifeIcons();
    }

    void Update()
    {
        if (!gameEnded)
        {
            UpdateTimer();
        }
    }

    public void SetQuestions(ExorcismQuestion[] newQuestions)
    {
        questions = newQuestions;
        currentQuestionIndex = 0;  // 重置問題索引
        lives = 3;  // 重置生命值
        timeRemaining = 30f;  // 重置計時器
        gameEnded = false;  // 重置遊戲狀態
        DisplayQuestion();  // 顯示第一題
    }

    void DisplayQuestion()
    {
        ExorcismQuestion currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            // 清除之前的所有監聽器，防止重複觸發
            answerButtons[i].onClick.RemoveAllListeners();

            // 判斷當前按鈕是否有答案，如果問題的答案少於按鈕數量，則隱藏多餘的按鈕
            if (i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];

                // 使用本地變數捕獲 i 的當前值
                int answerIndex = i;
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void CheckAnswer(int selectedAnswer)
    {
        if (questions[currentQuestionIndex].correctAnswerIndex == selectedAnswer)
        {
            // 答對，進入下一題
            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Length)
            {
                DisplayQuestion();
            }
            else
            {
                EndGame(true); // 通關
            }
        }
        else
        {
            lives--;
            UpdateLifeIcons();
            if (lives <= 0)
            {
                EndGame(false); // 遊戲失敗
            }
        }
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            slider.value = timeRemaining;
            timerText.text = "" + Mathf.Round(timeRemaining);
        }
        else
        {
            lives--;
            UpdateLifeIcons();

            if (lives <= 0)
            {
                EndGame(false); // 生命歸零，結束遊戲
            }
            else
            {
                timeRemaining = 30f; // 重置計時器
                DisplayQuestion();   // 顯示下一題
            }
        }
    }

    void UpdateLifeIcons()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < lives)
            {
                lifeIcons[i].color = Color.red;  // 剩餘生命設置為紅色
            }
            else
            {
                lifeIcons[i].color = Color.gray; // 損失生命設置為灰色
            }
        }
    }

    void EndGame(bool won)
    {

        gameEnded = true;

        // 停用按鈕
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }

        if (won)
        {
            Debug.Log("You won!"); // 顯示勝利訊息
        }
        else
        {
            Debug.Log("Game over!"); // 顯示失敗訊息
        }

        // 開始淡出面板
        StartCoroutine(FadeOutPanel());
    }

    public void ShowPanel()
    {
        // 重置按鈕的 interactable 狀態
        foreach (Button btn in answerButtons)
        {
            btn.interactable = true;  // 確保按鈕可以再次互動
        }

        panelCanvasGroup.alpha = 0;  // 從透明開始
        panelCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;
        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        float fadeDuration = 2.0f; // 淡入持續時間
        float startAlpha = panelCanvasGroup.alpha;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1, normalizedTime);  // 從透明變到完全不透明
            yield return null;
        }

        panelCanvasGroup.alpha = 1;  // 確保最後完全可見
    }

    IEnumerator FadeOutPanel()
    {
        float fadeDuration = 2.0f; // 淡出持續時間
        float startAlpha = panelCanvasGroup.alpha;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime);
            yield return null;
        }

        panelCanvasGroup.alpha = 0;  // 最後確保完全隱藏
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;
    }
}
