using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Collections;

public class ModalWindowManager : MonoBehaviour
{
    public static ModalWindowManager instance;

    [SerializeField] private Transform panel;
    [SerializeField] private Transform modalWindow;

    [Header("Header")]
    [SerializeField] private Transform headerArea;
    [SerializeField] private TextMeshProUGUI headerTitle;

    [Header("Content")]
    [SerializeField] private Transform contentArea;
    [Space(5)]
    [SerializeField] private Transform verticalLayoutArea;
    [SerializeField] private Image verticalImage;
    [SerializeField] private TextMeshProUGUI verticalContent;
    [Space(5)]
    [SerializeField] private Transform horizontalLayoutArea;
    [SerializeField] private Image horizontalImage;
    [SerializeField] private TextMeshProUGUI horizontalContent;

    [Header("Footer")]
    [SerializeField] private Transform footerArea;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;
    [SerializeField] private Button declineButton;
    [SerializeField] private TextMeshProUGUI declineButtonText;
    [SerializeField] private Button alternateButton;
    [SerializeField] private TextMeshProUGUI alternateButtonText;

    private Action onConfirmAction;
    private Action onDeclineAction;
    private Action onAlternateAction;

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
    }

    void Start()
    {
        panel.gameObject.SetActive(false);
        modalWindow.gameObject.SetActive(false);
    }

    public void Close()
    {
        panel.gameObject.SetActive(false);
        modalWindow.gameObject.SetActive(false);
        TestSceneManager.instance.buttonInteruption = false;
    }

    public void ShowVertical(string title, Sprite image, string content, string confirmText, Action confirmAction, string declineText = null, Action declineAction = null, string alternateText = null, Action alternateAction = null)
    {
        panel.gameObject.SetActive(true);
        modalWindow.gameObject.SetActive(true);
        horizontalLayoutArea.gameObject.SetActive(false);
        verticalLayoutArea.gameObject.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();
        alternateButton.onClick.RemoveAllListeners();

        // �]�m���D�ϰ�
        bool hasTitle = !string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(hasTitle);
        headerTitle.text = title;

        // �]�m�Ϥ�
        bool hasImage = (image != null);
        verticalImage.gameObject.SetActive(hasImage);
        if (hasImage)
        {
            verticalImage.sprite = image;
        }
        else
        {
            verticalImage.sprite = null;
        }

        // �]�m���e��r
        verticalContent.text = content;

        // �]�m�T�{���s
        confirmButton.gameObject.SetActive(true);
        confirmButtonText.text = confirmText;
        confirmButton.onClick.AddListener(new UnityAction(confirmAction));

        // �]�m�ڵ����s
        bool hasDecline = !string.IsNullOrEmpty(declineText);
        declineButton.gameObject.SetActive(hasDecline);
        if (hasDecline)
        {
            if (declineText != null)
            {
                declineButtonText.text = declineText;
            }
            declineButton.onClick.AddListener(new UnityAction(declineAction));
        }

        // �]�m�ƿ���s
        bool hasAlternate = !string.IsNullOrEmpty(alternateText);
        alternateButton.gameObject.SetActive(hasAlternate);
        if (hasAlternate)
        {
            if (alternateText != null)
            {
                alternateButtonText.text = alternateText;
            }
            alternateButton.onClick.AddListener(new UnityAction(alternateAction));
        }

        TestSceneManager.instance.buttonInteruption = true;
        StartCoroutine(DelayedShow());
    }

    public void ShowHorizontal(string title, Sprite image, string content, string confirmText, Action confirmAction, string declineText = null, Action declineAction = null, string alternateText = null, Action alternateAction = null)
    {
        modalWindow.gameObject.SetActive(true);
        horizontalLayoutArea.gameObject.SetActive(true);
        verticalLayoutArea.gameObject.SetActive(false);

        confirmButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();
        alternateButton.onClick.RemoveAllListeners();

        // �]�m���D�ϰ�
        bool hasTitle = !string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(hasTitle);
        headerTitle.text = title;

        // �]�m�Ϥ�
        bool hasImage = (image != null);
        horizontalImage.gameObject.SetActive(hasImage);
        if (hasImage)
        {
            horizontalImage.sprite = image;
        }
        else
        {
            horizontalImage.sprite = null;
        }

        // �]�m���e��r
        horizontalContent.text = content;

        // �]�m�T�{���s
        confirmButton.gameObject.SetActive(true);
        confirmButtonText.text = confirmText;
        confirmButton.onClick.AddListener(new UnityAction(confirmAction));

        // �]�m�ڵ����s
        bool hasDecline = !string.IsNullOrEmpty(declineText);
        declineButton.gameObject.SetActive(hasDecline);
        if (hasDecline)
        {
            if (declineText != null)
            {
                declineButtonText.text = declineText;
            }
            declineButton.onClick.AddListener(new UnityAction(declineAction));
        }

        // �]�m�ƿ���s
        bool hasAlternate = !string.IsNullOrEmpty(alternateText);
        alternateButton.gameObject.SetActive(hasAlternate);
        if (hasAlternate)
        {
            if (alternateText != null)
            {
                alternateButtonText.text = alternateText;
            }
            alternateButton.onClick.AddListener(new UnityAction(alternateAction));
        }

        TestSceneManager.instance.buttonInteruption = true;
        StartCoroutine(DelayedShow());
    }

    private IEnumerator DelayedShow()
    {
        yield return null; // ���ݤ@�V

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)modalWindow);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)horizontalLayoutArea);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutArea);
    }
}
