using UnityEngine;

public class CluePickup : ButtonBaseFunction
{
    public ClueData clue;

    protected override void OnMouseDown()
    {
        if (!ProgressManager.instance.buttonInteruption)
        {
            InventoryManager.instance.Add(clue);
            Destroy(gameObject);
        }
    }
}
