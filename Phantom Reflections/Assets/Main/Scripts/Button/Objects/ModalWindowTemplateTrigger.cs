using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalWindowTemplateTrigger : ButtonBaseFunction
{
    [SerializeField] private ModalWindowTemplate[] modalWindowTemplates;

    private int currentIndex = 0;

    protected override void OnMouseDown()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            currentIndex = 0; // ���m����
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
                ShowCurrentModal(); // ��ܤU�@��
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
