using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalWindowTemplateTrigger : ButtonBaseFunction
{
    [SerializeField] private ModalWindowTemplate[] modalWindowTemplates;

    private int currentIndex = 0;

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            currentIndex = 0; // 重置索引
            ShowCurrentModal();
        }

        base.OnMouseDown();
    }

    private void ShowCurrentModal()
    {
        if (currentIndex >= modalWindowTemplates.Length)
        {
            ModalWindowManager.instance.Close();
            return;
        }

        ModalWindowTemplate currentTemplate = modalWindowTemplates[currentIndex];

        ModalWindowManager.instance.ShowVertical(
            currentTemplate.title,
            currentTemplate.image,
            currentTemplate.context,
            currentTemplate.confirmText, () =>
            {
                currentIndex++;
                ModalWindowManager.instance.Close();
                ShowCurrentModal(); // 顯示下一個
            },
            currentTemplate.declineText, () =>
            {
                ModalWindowManager.instance.Close();
            },
            currentTemplate.alternateText, () =>
            {
                ModalWindowManager.instance.Close();
            }
        );
    }
}
