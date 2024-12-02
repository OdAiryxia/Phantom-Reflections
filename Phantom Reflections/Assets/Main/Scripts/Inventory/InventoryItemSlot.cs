using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject stackObj;
    [SerializeField] private TextMeshProUGUI stackSize;

    private InventoryClue inventoryClue;

    public void Set(InventoryClue clue)
    {
        inventoryClue = clue;

        icon.sprite = clue.clueData.sprite;
        label.text = clue.clueData.clueName;
        if (clue.stackSize <= 1)
        {
            stackObj.SetActive(false);
            return;
        }
        else
        {
            stackObj.SetActive(true);
        }
        stackSize.text = clue.stackSize.ToString();
    }

    public void OnClueButtonClicked()
    {
        InventoryUI.instance.ShowClueDetails(
            inventoryClue.clueData.sprite,
            inventoryClue.clueData.clueName,
            inventoryClue.clueData.description
        );
    }
}
