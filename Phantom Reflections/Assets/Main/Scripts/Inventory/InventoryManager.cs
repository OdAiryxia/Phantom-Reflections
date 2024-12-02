using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;


    private Dictionary<ClueData, InventoryClue> m_clueDictionary;
    public List<InventoryClue> inventory;

    public Action onInventoryChangedEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        inventory = new List<InventoryClue>();
        m_clueDictionary = new Dictionary<ClueData, InventoryClue>();
    }
    public InventoryClue Get(ClueData referenceData)
    {
        if (m_clueDictionary.TryGetValue(referenceData, out InventoryClue value))
        {
            return value;
        }
        return null;
    }

    public void Add(ClueData referenceData)
    {
        if (m_clueDictionary.TryGetValue(referenceData, out InventoryClue value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryClue newItem = new InventoryClue(referenceData);
            inventory.Add(newItem);
            m_clueDictionary.Add(referenceData, newItem);
        }

        onInventoryChangedEvent?.Invoke();
    }

    public void Remove(ClueData referenceData)
    {
        if (m_clueDictionary.TryGetValue(referenceData, out InventoryClue value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_clueDictionary.Remove(referenceData);
            }
        }

        onInventoryChangedEvent?.Invoke();
    }
}
