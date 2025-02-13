using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableClue : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(originalParent.root); // 讓它不受父物件影響
        canvasGroup.blocksRaycasts = false; // 避免擋住 Drop 事件
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; // 讓按鈕跟隨滑鼠
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // 拖曳結束，回到原本的父物件
        canvasGroup.blocksRaycasts = true; // 重新啟用點擊
    }
}
