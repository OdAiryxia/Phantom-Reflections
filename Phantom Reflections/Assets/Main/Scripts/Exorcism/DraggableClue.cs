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
        transform.SetParent(originalParent.root); // ��������������v�T
        canvasGroup.blocksRaycasts = false; // �קK�צ� Drop �ƥ�
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; // �����s���H�ƹ�
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // �즲�����A�^��쥻��������
        canvasGroup.blocksRaycasts = true; // ���s�ҥ��I��
    }
}
