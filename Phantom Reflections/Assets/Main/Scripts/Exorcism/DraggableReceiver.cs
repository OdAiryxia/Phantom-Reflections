using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        if (id != null)
        {
            if (draggedClue != null && (id == inventoryItemSlot.id))
            {
                Destroy(draggedClue.gameObject);
                Debug.Log("按鈕 " + draggedClue.name + " 被拖放到 " + gameObject.name);

                if (newAnswer != null)    
                {
                    TextMeshProUGUI textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
                    textMeshProUGUI.text = newAnswer;
                }
            }
        }
    }
}
