using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Exorcism/Question")]
public class ExorcismQuestion : ScriptableObject
{
    [TextArea]
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex;
}
