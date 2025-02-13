using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ExorcismManager : MonoBehaviour
{
    public static ExorcismManager instance;

    [SerializeField] private CanvasGroup canvasGroup;

    [Header("除靈題目")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private ExorcismQuestion[] questions;
    private int currentQuestionIndex = 0;

    [Header("選項")]
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private RectTransform[] optionParents;
    private List<Vector2> spawnedPositions = new List<Vector2>();

    [Header("線索")]
    [SerializeField] private ScrollRect clueBar;
    [SerializeField] private GameObject cluePrefab;
    [SerializeField] private Transform clueParent;

    [Header("時間")]
    [SerializeField] private Slider timerSlider;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeRemaining = 30f;

    public bool onExorcismProgress = false;

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
    }

    void Update()
    {
        if (onExorcismProgress)
        {
            UpdateTimer();
        }
    }

    public void SetQuestion(ExorcismQuestion[] newQuestions)
    {
        TestSceneManager.instance.buttonInteruption = true;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        onExorcismProgress = true;
        clueBar.enabled = true;

        timeRemaining = 30f;
        timerText.color = Color.white;

        questions = newQuestions;
        currentQuestionIndex = 0;

        InventoryUI.instance.Hide();

        TextWiggleEffect timerWiggleEffect = timerText.GetComponent<TextWiggleEffect>();
        timerWiggleEffect.shakeAmount = -2f;
        timerWiggleEffect.speed = 10f;

        foreach (Transform child in clueParent)
        {
            Destroy(child.gameObject);
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
        ExorcismQuestion currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;

        timeRemaining = 30f;
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
            }

            DraggableReceiver draggableReceiver = newPrefab.GetComponent<DraggableReceiver>();
            if (draggableReceiver != null)
            {
                draggableReceiver.id = currentQuestion.answers[index].id;
                draggableReceiver.newAnswer = currentQuestion.answers[index].newAnswer;
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

        if (timeRemaining == 0)
        {
            EndGame(false);
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
            Debug.Log("You won!");
        }
        else
        {
            Debug.Log("Game over!");
        }

        yield return StartCoroutine(BlackScreenManager.instance.Transition(canvasGroup));
        onExorcismProgress = false;
        TestSceneManager.instance.buttonInteruption = false;
    }
}
