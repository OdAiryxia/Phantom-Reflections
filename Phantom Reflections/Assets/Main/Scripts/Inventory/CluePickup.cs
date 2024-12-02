using UnityEngine;

public class CluePickup : MonoBehaviour
{
    public ClueData clue;

    public void OnClicked()
    {
        InventoryManager.instance.Add(clue);
        Destroy(gameObject);
    }
}
