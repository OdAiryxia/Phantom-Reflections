using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class ExorcismManager : MonoBehaviour
{
    public static ExorcismManager instance;

    [Header("題庫與狀態")]
    public Image opponentImage;
    public TMP_Text osText;

    public ExorcismQuestion[] questions;
    public ExorcismQuestion[] activeQuestions; // 複製用
    public int currentQuestionIndex = 0;
    public bool onExorcismProgress = false;
    public UnityEvent events;
    [SerializeField] private TextMeshProUGUI title;

    [Header("除靈題目")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private RectTransform[] optionParents;

    [Header("線索")]
    [SerializeField] private ScrollRect clueBar;
    [SerializeField] private GameObject cluePrefab;
    [SerializeField] private Transform clueParent;

    [Header("計時器")]
    [SerializeField] private Slider timerSlider;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeRemaining = 60f;

    [Header("Overlay效果")]
    [SerializeField] private Sprite[] stage_one;
    [SerializeField] private Sprite[] stage_two;
    [SerializeField] private Sprite[] stage_three;
    [Space(5)]
    [SerializeField] private Image stage_one_image;
    [SerializeField] private Image stage_two_image;
    [SerializeField] private Image stage_three_image;
    [SerializeField] private CanvasGroup vignette;
    [SerializeField] private PostProcessVolume postProcessing;
    [Space(5)]
    public int overlayStage = 0;

    private bool isEnding = false;

    private List<Vector2> spawnedPositions = new List<Vector2>();

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

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        stage_one_image.enabled = false;
        stage_two_image.enabled = false;
        stage_three_image.enabled = false;
        vignette.alpha = 0f;
        postProcessing.weight = 0f;
    }

    void Update()
    {
        if (onExorcismProgress)
        {
            UpdateTimer();
        }
    }

    public void SetQuestion()
    {
        ProgressManager.instance.buttonInteruption = true;
        InventoryUI.instance.Hide();

        isEnding = false;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        onExorcismProgress = true;
        clueBar.enabled = true;
        timeRemaining = 60f;
        timerText.color = Color.white;

        title.text = "除靈";

        currentQuestionIndex = 0;

        TextWiggleEffect timerWiggleEffect = timerText.GetComponent<TextWiggleEffect>();
        timerWiggleEffect.shakeAmount = -2f;
        timerWiggleEffect.speed = 10f;

        foreach (Transform child in clueParent)
        {
            Destroy(child.gameObject);
        }

        activeQuestions = new ExorcismQuestion[questions.Length];
        for (int i = 0; i < questions.Length; i++)
        {
            ExorcismQuestion source = questions[i];
            ExorcismQuestion copy = ScriptableObject.CreateInstance<ExorcismQuestion>();
            copy.questionText = source.questionText;
            copy.protaginistOS = source.protaginistOS;

            answers[] copiedAnswers = new answers[source.answers.Length];
            for (int j = 0; j < copiedAnswers.Length; j++)
            {
                answers src = source.answers[j];
                copiedAnswers[j] = new answers
                {
                    answer = src.answer,
                    answerOS = src.answerOS,
                    isCorrect = src.isCorrect,
                    id = src.id,
                    newAnswer = src.newAnswer,
                    newAnswerOS = src.newAnswerOS,
                    isNewCorrect = src.isNewCorrect
                };
            }
            copy.answers = copiedAnswers;
            activeQuestions[i] = copy;
        }


        foreach (InventoryClue clue in InventoryManager.instance.inventory)
        {
            GameObject obj = Instantiate(cluePrefab);
            obj.transform.SetParent(clueParent, false);

            InventoryItemSlot slot = obj.GetComponent<InventoryItemSlot>();
            slot.Set(clue);
        }

        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        ExorcismQuestion currentQuestion = activeQuestions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;

        if (currentQuestion.protaginistOS != null && currentQuestion.protaginistOS.Length > 0)
        {
            osText.text = currentQuestion.protaginistOS[Random.Range(0, currentQuestion.protaginistOS.Length)];
            LayoutRebuilder.ForceRebuildLayoutImmediate(osText.GetComponentInParent<RectTransform>());
        }
        else
        {
            osText.text = "...";
            LayoutRebuilder.ForceRebuildLayoutImmediate(osText.GetComponentInParent<RectTransform>());
        }

        timeRemaining = 60f;
        timerText.color = Color.white;

        foreach (Transform parent in optionParents)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

        spawnedPositions.Clear();

        int generatedCount = 0;

        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            RectTransform targetParent = optionParents[i % optionParents.Length];

            Vector2 newPoint = GetRandomPointInImage(targetParent);

            // 可選擇是否啟用 overlapping 檢查
            bool overlapping = true;
            float xMinSpacing = 200f;
            float yMinSpacing = 100f;

            foreach (var pos in spawnedPositions)
            {
                if (Mathf.Abs(pos.x - newPoint.x) < xMinSpacing && Mathf.Abs(pos.y - newPoint.y) < yMinSpacing)
                {
                    overlapping = true;
                    break;
                }
            }

            if (!overlapping || optionParents.Length > 1)
            {
                spawnedPositions.Add(newPoint);
                SpawnPrefabAt(newPoint, targetParent, i, currentQuestion);
                generatedCount++;
            }
        }
    }

    Vector2 GetRandomPointInImage(RectTransform parent)
    {
        Vector3[] worldCorners = new Vector3[4];
        parent.GetWorldCorners(worldCorners);

        float randomX = Random.Range(worldCorners[0].x, worldCorners[2].x);
        float randomY = Random.Range(worldCorners[0].y, worldCorners[2].y);

        return new Vector2(randomX, randomY);
    }

    void SpawnPrefabAt(Vector2 worldPosition, RectTransform parent, int index, ExorcismQuestion currentQuestion)
    {
        GameObject newPrefab = Instantiate(optionPrefab, parent);
        newPrefab.transform.localPosition = parent.InverseTransformPoint(worldPosition);

        if (index < currentQuestion.answers.Length)
        {
            TextMeshProUGUI textComponent = newPrefab.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = currentQuestion.answers[index].answer;
                textComponent.color = Color.white;
            }

            DraggableReceiver draggableReceiver = newPrefab.GetComponent<DraggableReceiver>();
            if (draggableReceiver != null)
            {
                draggableReceiver.isNewAnswer = false;

                draggableReceiver.answer = currentQuestion.answers[index].answer;
                draggableReceiver.answerOS = currentQuestion.answers[index].answerOS;
                draggableReceiver.isCorrect = currentQuestion.answers[index].isCorrect;
                draggableReceiver.id = currentQuestion.answers[index].id;
                draggableReceiver.newAnswer = currentQuestion.answers[index].newAnswer;
                draggableReceiver.newAnswerOS = currentQuestion.answers[index].newAnswerOS;
                draggableReceiver.isNewCorrect = currentQuestion.answers[index].isNewCorrect;
            }

            Button button = newPrefab.GetComponent<Button>();
            if (button != null)
            {
                int answerIndex = index;
                button.onClick.AddListener(() => CheckAnswer(answerIndex));
            }
        }

        FloatingText floatingText = newPrefab.GetComponent<FloatingText>();
        if (floatingText != null)
        {
            floatingText.floatRange *= (Random.Range(0, 2) == 0 ? -1 : 1);
            floatingText.floatRange += Random.Range(-0.3f, 0.3f);

            floatingText.floatSpeed *= (Random.Range(0, 2) == 0 ? -1 : 1);
            floatingText.floatSpeed += Random.Range(-0.3f, 0.3f);

            floatingText.rotateRange *= (Random.Range(0, 2) == 0 ? -1 : 1);
            floatingText.rotateRange += Random.Range(-0.3f, 0.3f);

            floatingText.rotateSpeed *= (Random.Range(0, 2) == 0 ? -1 : 1);
            floatingText.rotateSpeed += Random.Range(-0.3f, 0.3f);
        }
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0)
            {
                timeRemaining = 0;
            }

            timerSlider.value = timeRemaining;
            timerText.text = "" + Mathf.Round(timeRemaining);
        }

        // 根據剩餘時間變換顏色
        if (timeRemaining <= 5)
        {
            Color midColor1 = new Color(1f, 0.6f, 0.6f);
            Color midColor2 = new Color(1f, 0.4f, 0.4f);
            Color endColor = Color.red;

            if (timeRemaining <= 5 && timeRemaining > 4)
            {
                timerText.color = midColor1;
            }
            else if (timeRemaining <= 4 && timeRemaining > 3)
            {
                timerText.color = midColor2;
            }
            else if (timeRemaining <= 3)
            {
                timerText.color = endColor;
            }
        }
        else
        {
            timerText.color = Color.white;
        }

        if (timeRemaining <= 0)
        {
            StartCoroutine(EndGame(false));
        }
    }

    void CheckAnswer(int selectedAnswer)
    {
        ExorcismQuestion currentQuestion = activeQuestions[currentQuestionIndex];
        answers selected = currentQuestion.answers[selectedAnswer];

        if (selected.isCorrect)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < activeQuestions.Length)
            {
                DisplayQuestion();
            }
            else
            {
                StartCoroutine(EndGame(true)); // 通關
            }
        }
        else
        {
            timeRemaining -= 10f;

            if (timeRemaining < 0)
            {
                timeRemaining = 0;
                timerText.text = "0";
                timerSlider.value = 0;
                StartCoroutine(EndGame(false));
            }
        }
    }

    IEnumerator EndGame(bool won)
    {
        if (isEnding) yield break; // 若已經在結束流程中，就跳出
        isEnding = true;           // 設定正在結束狀態

        clueBar.enabled = false;

        foreach (Transform child in clueParent)
        {
            DraggableClue draggableClue = child.GetComponent<DraggableClue>();
            draggableClue.enabled = false;
        }

        foreach (Transform parent in optionParents)
        {
            foreach (Transform child in parent)
            {
                Button button = child.GetComponent<Button>();
                FloatingText floatingText = child.GetComponent<FloatingText>();
                TextWiggleEffect optionsWiggleEffect = child.GetComponentInChildren<TextWiggleEffect>();

                if (button != null)
                {
                    button.interactable = false;
                }

                if (floatingText != null)
                {
                    floatingText.enabled = false;
                }

                if (optionsWiggleEffect != null)
                {
                    optionsWiggleEffect.enabled = false;
                }
            }
        }

        if (!won)
        {
            TextWiggleEffect timerWiggleEffect = timerText.GetComponent<TextWiggleEffect>();
            StartCoroutine(TimerCrashAnimation(timerWiggleEffect, timerWiggleEffect.speed * 5, 0, 2f));
        }

        IEnumerator TimerCrashAnimation(TextWiggleEffect effect, float startSpeed, float endSpeed, float duration)
        {
            float initialShake = effect.shakeAmount;
            timerText.color = Color.red;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                t = 1 - Mathf.Pow(1 - t, 3);

                effect.speed = Mathf.Lerp(startSpeed, endSpeed, t);
                effect.shakeAmount = Mathf.Lerp(Random.Range(initialShake * 1f, initialShake * 10f), initialShake, t);

                yield return null;
            }

            effect.speed = endSpeed;
            effect.shakeAmount = initialShake;
        }

        if (won)
        {
            title.text = "成功";
        }
        else
        {
            events = null;
            EnableOverlayStage();
            title.text = "失敗";
        }

        yield return StartCoroutine(BlackScreenManager.instance.Transition(canvasGroup));
        onExorcismProgress = false;
        ProgressManager.instance.buttonInteruption = false;
    }

    #region Overlay
    public void EnableOverlayStage()
    {
        switch (overlayStage)
        {
            case 0:
                stage_one_image.enabled = true;
                StartCoroutine(OverlayLoop(stage_one, stage_one_image));
                vignette.alpha = 0.25f;
                postProcessing.weight = 0.1f;
                break;
            case 1:
                stage_two_image.enabled = true;
                StartCoroutine(OverlayLoop(stage_two, stage_two_image));
                vignette.alpha = 0.5f;
                postProcessing.weight = 0.25f;
                break;
            case 2:
                stage_three_image.enabled = true;
                StartCoroutine(OverlayLoop(stage_three, stage_three_image));
                vignette.alpha = 1f;
                postProcessing.weight = 1f;
                break;
            default:
                break;
        }

        overlayStage++;
    }

    IEnumerator OverlayLoop(Sprite[] sprites, Image image)
    {
        List<int> order = new List<int>();

        for (int i = 0; i < sprites.Length; i++) order.Add(i);
        for (int i = sprites.Length - 2; i > 0; i--) order.Add(i);

        int index = 0;

        while (true)
        {
            image.sprite = sprites[order[index]];
            index = (index + 1) % order.Count;
            yield return new WaitForSeconds(Random.Range(0.16f,0.20f));
        }
    }
    #endregion
}
