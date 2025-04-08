using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableReceiver : MonoBehaviour, IDropHandler
{
    [Header("線索ID")]
    public string id;
    public string newAnswer;
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
                    // 將此選項設為正確答案
                    answer.isCorrect = true;
                    break;
                }
            }
        }
    }
}
