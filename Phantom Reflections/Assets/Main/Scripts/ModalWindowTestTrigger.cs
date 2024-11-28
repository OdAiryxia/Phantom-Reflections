using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ModalWindowTemplate
{
    public string title;
    public Sprite image;
    [TextArea(3, 10)] public string context;

    public string confirmText;
    public string declineText;
    public string alternateText;
}

public class ModalWindowTestTrigger : MonoBehaviour
{
    [SerializeField] private ModalWindowTemplate[] modalWindowTemplates;

    private int currentIndex = 0;

    public void Trigger()
    {
        currentIndex = 0; // 重置索引
        ShowCurrentModal();
    }

    private void ShowCurrentModal()
    {
        if (currentIndex >= modalWindowTemplates.Length)
        {
            Debug.Log("All modal windows are finished.");
            ModalWindow.Instance.Close();
            return;
        }

        ModalWindowTemplate currentTemplate = modalWindowTemplates[currentIndex];

        ModalWindow.Instance.ShowVertical(
            currentTemplate.title,
            currentTemplate.image,
            currentTemplate.context,
            currentTemplate.confirmText, () =>
            {
                Debug.Log("Confirmed: " + currentTemplate.title);
                currentIndex++;
                ModalWindow.Instance.Close();
                ShowCurrentModal(); // 顯示下一個
            },
            currentTemplate.declineText, () =>
            {
                Debug.Log("Declined: " + currentTemplate.title);
                ModalWindow.Instance.Close();
            },
            currentTemplate.alternateText, () =>
            {
                Debug.Log("Alternate action: " + currentTemplate.title);
                ModalWindow.Instance.Close();
            }
        );
    }
}
