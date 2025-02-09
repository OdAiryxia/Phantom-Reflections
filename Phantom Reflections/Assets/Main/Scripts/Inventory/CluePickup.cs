using UnityEngine;

public class CluePickup : ButtonBaseFunction
{
    public ClueData clue;

    protected override void OnMouseDown()
    {
        if (!TestSceneManager.instance.buttonInteruption)
        {
            InventoryManager.instance.Add(clue);
            Destroy(gameObject);
        }
    }
}
