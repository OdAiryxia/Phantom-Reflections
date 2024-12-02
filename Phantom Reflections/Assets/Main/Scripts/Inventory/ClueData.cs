using UnityEngine;

[CreateAssetMenu(fileName = "NewClue", menuName = "Inventory/Clue")]
public class ClueData : ScriptableObject
{
    public string id;
    public Sprite sprite;
    public string clueName;
    [TextArea] public string description;
}
