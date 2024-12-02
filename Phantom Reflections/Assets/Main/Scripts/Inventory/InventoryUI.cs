using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public GameObject slotPrefeb;
    [SerializeField] private Transform slotParent;

    [SerializeField] private GameObject itemInspectorPanel;
    [SerializeField] private Image clueImage;
    [SerializeField] private TextMeshProUGUI clueNameText;
    [SerializeField] private TextMeshProUGUI clueDescriptionText;


    private void Awake()
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
    }
    private void Start()
    {
        InventoryManager.instance.onInventoryChangedEvent += OnUpdateInventory;
    }

    private void OnUpdateInventory()
    {
        foreach(Transform t in slotParent)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach(InventoryClue clue in InventoryManager.instance.inventory)
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
    }
}
