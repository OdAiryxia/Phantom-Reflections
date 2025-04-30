using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        if (ExorcismManager.instance.onExorcismProgress)
        {
            ExorcismQuestion currentQuestion = ExorcismManager.instance.activeQuestions[ExorcismManager.instance.currentQuestionIndex];
            if (currentQuestion.protaginistOS != null && currentQuestion.protaginistOS.Length > 0)
            {
                ExorcismManager.instance.osText.text = currentQuestion.protaginistOS[Random.Range(0, currentQuestion.protaginistOS.Length)];
                LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
            }
            else
            {
                ExorcismManager.instance.osText.text = "...";
                LayoutRebuilder.ForceRebuildLayoutImmediate(ExorcismManager.instance.osText.GetComponentInParent<RectTransform>());
            }
        }
    }
}
