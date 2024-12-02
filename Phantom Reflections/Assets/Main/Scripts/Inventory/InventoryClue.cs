using System;

[Serializable]
public class InventoryClue
{
    public ClueData clueData;
    public int stackSize;
    public InventoryClue(ClueData source)
    {
        clueData = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
