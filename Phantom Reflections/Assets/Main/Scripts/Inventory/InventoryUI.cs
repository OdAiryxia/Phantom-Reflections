using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public enum ShowState
    {
        Show,
        Hide
    }

    public static InventoryUI instance;

    public ShowState currentState = ShowState.Hide;
    [SerializeField] private CanvasGroup barCanvasGroup;

    [SerializeField] private GameObject slotPrefeb;
    [SerializeField] private Transform slotParent;

    [SerializeField] private GameObject itemInspectorPanel;
    [SerializeField] private Image clueImage;
    [SerializeField] private TextMeshProUGUI clueNameText;
    [SerializeField] private TextMeshProUGUI clueDescriptionText;

    [HideInInspector] public bool onInventoryInspector;

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

        itemInspectorPanel.SetActive(false);
        Hide();
    }
    void Start()
    {
        InventoryManager.instance.onInventoryChangedEvent += OnUpdateInventory;
    }

    void Update()
    {
        if (!ExorcismManager.instance.onExorcismProgress)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleShow();
            }
        }
    }

    public void ToggleShow()
    {
        switch (currentState)
        {
            case ShowState.Show:
                Hide();
                break;

            case ShowState.Hide:
                Show();
                break;
        }
    }

    public void Show()
    {
        barCanvasGroup.alpha = 1f;
        barCanvasGroup.interactable = true;
        barCanvasGroup.blocksRaycasts = true;
        currentState = ShowState.Show;
    }

    public void Hide()
    {
        barCanvasGroup.alpha = 0f;
        barCanvasGroup.interactable = false;
        barCanvasGroup.blocksRaycasts = false;
        currentState = ShowState.Hide;
    }

    void OnUpdateInventory()
    {
        foreach (Transform t in slotParent)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryClue clue in InventoryManager.instance.inventory)
        {
            AddInventorySlot(clue);
        }
    }

    public void AddInventorySlot(InventoryClue clue)
    {
        GameObject obj = Instantiate(slotPrefeb);
        obj.transform.SetParent(slotParent, false);

        InventoryItemSlot slot = obj.GetComponent<InventoryItemSlot>();
        slot.Set(clue);
    }

    public void ShowClueDetails(Sprite image, string name, string description)
    {
        clueImage.sprite = image;
        clueNameText.text = name;
        clueDescriptionText.text = description;
        itemInspectorPanel.SetActive(true);
    }

    public void HideClueDetails()
    {
        itemInspectorPanel.SetActive(false);
        PauseManager.instance.pauseButton.onClick.RemoveAllListeners();
        PauseManager.instance.pauseButton?.onClick.AddListener(PauseManager.instance.TogglePause);
        PauseManager.instance.pauseButton.image.sprite = PauseManager.instance.pauseImage;

        ProgressManager.instance.buttonInteruption = false;
        onInventoryInspector = false;
        Time.timeScale = 1f;
    }
}
