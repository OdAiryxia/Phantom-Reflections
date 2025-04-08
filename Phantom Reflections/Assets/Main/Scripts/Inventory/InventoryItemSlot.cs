using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour
{
    public string id;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject stackObject;
    [SerializeField] private TextMeshProUGUI stackSize;

    private InventoryClue inventoryClue;

    public void Set(InventoryClue clue)
    {
        inventoryClue = clue;

        id = clue.clueData.id;
        icon.sprite = clue.clueData.sprite;
        label.text = clue.clueData.clueName;
        if (clue.stackSize <= 1)
        {
            stackObject.SetActive(false);
            return;
        }
        else
        {
            stackObject.SetActive(true);
        }
        stackSize.text = clue.stackSize.ToString();
    }

    public void OnClueButtonClicked()
    {
        if (!ProgressManager.instance.buttonInteruption || InventoryUI.instance.onInventoryInspector)
        {
            InventoryUI.instance.ShowClueDetails(inventoryClue.clueData.sprite, inventoryClue.clueData.clueName, inventoryClue.clueData.description);

            PauseManager.instance.pauseButton.onClick.RemoveAllListeners();
            PauseManager.instance.pauseButton.onClick.AddListener(InventoryUI.instance.HideClueDetails);
            PauseManager.instance.pauseButton.image.sprite = PauseManager.instance.exitImage;

            ProgressManager.instance.buttonInteruption = true;
            InventoryUI.instance.onInventoryInspector = true;
            Time.timeScale = 0; Time.timeScale = 0f;
        }

    }
}
