using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class DraggableReceiver : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("線索ID")]
    public bool isNewAnswer;

    public string answer;
    public string answerOS;
    public bool isCorrect;
    public string id;
    public string newAnswer;
    public string newAnswerOS;
    public bool isNewCorrect;
    public void OnDrop(PointerEventData eventData)
    {
        DraggableClue draggedClue = eventData.pointerDrag.GetComponent<DraggableClue>();
        InventoryItemSlot inventoryItemSlot = eventData.pointerDrag.GetComponent<InventoryItemSlot>();

        ExorcismQuestion currentQuestion = ExorcismManager.instance.activeQuestions[ExorcismManager.instance.currentQuestionIndex];

        if (id != null && draggedClue != null && id == inventoryItemSlot.id)
        {
            Destroy(draggedClue.gameObject);
            Debug.Log("按鈕 " + draggedClue.name + " 被拖放到 " + gameObject.name);

            // 改變按鈕文字
            if (!string.IsNullOrEmpty(newAnswer))
            {
                TextMeshProUGUI textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
                if (textMeshProUGUI != null)
                {
                    textMeshProUGUI.text = newAnswer;
                    textMeshProUGUI.color = Color.yellow;
                }
            }

            foreach (var answer in currentQuestion.answers)
            {
                // 比對這個選項是否是我們拖入線索的目標
                if (answer.id == id)
                {
                    isNewAnswer = true;
                    answer.isCorrect = answer.isNewCorrect;
                    break;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ExorcismManager.instance.onExorcismProgress)
        {
            if (isNewAnswer)
            {
                if (!string.IsNullOrEmpty(newAnswerOS))
                {
                    ExorcismManager.instance.osText.text = newAnswerOS;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
                }
            }
            else if (!isNewAnswer)
            {
                if (!string.IsNullOrEmpty(answerOS))
                {
                    ExorcismManager.instance.osText.text = answerOS;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        try
        {
            if (ExorcismManager.instance == null || !ExorcismManager.instance.onExorcismProgress)
            {
                return;
            }

            // 非常重要的索引检查
            if (ExorcismManager.instance.currentQuestionIndex < 0 ||
                ExorcismManager.instance.currentQuestionIndex >= ExorcismManager.instance.activeQuestions.Length)
            {
                if (ExorcismManager.instance.osText != null)
                {
                    ExorcismManager.instance.osText.text = "...";
                    LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
                }
                return;
            }

            ExorcismQuestion currentQuestion = ExorcismManager.instance.activeQuestions[ExorcismManager.instance.currentQuestionIndex];

            if (currentQuestion == null)
            {
                if (ExorcismManager.instance.osText != null)
                {
                    ExorcismManager.instance.osText.text = "...";
                    LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
                }
                return;
            }

            if (currentQuestion.protaginistOS != null && currentQuestion.protaginistOS.Length > 0)
            {
                int randomIndex = Random.Range(0, currentQuestion.protaginistOS.Length);
                if (randomIndex >= 0 && randomIndex < currentQuestion.protaginistOS.Length)
                {
                    ExorcismManager.instance.osText.text = currentQuestion.protaginistOS[randomIndex];
                }
                else
                {
                    ExorcismManager.instance.osText.text = "...";
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
            }
            else
            {
                ExorcismManager.instance.osText.text = "...";
                LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnPointerExit 發生異常: " + e.Message);
            if (ExorcismManager.instance != null && ExorcismManager.instance.osText != null)
            {
                ExorcismManager.instance.osText.text = "...";
                LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
            }
        }
    }
}
