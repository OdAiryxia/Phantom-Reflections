using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Question", menuName = "Exorcism/Question")]
public class ExorcismQuestion : ScriptableObject
{
    [TextArea]
    public string questionText;
    public answers[] answers;
}

[System.Serializable]
public class answers
{
    public string answer;
    public string id;
    public string newAnswer;
    public bool isCorrect;
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(answers))]
public class AnswersDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float space = 2f;

        SerializedProperty answer = property.FindPropertyRelative("answer");
        SerializedProperty id = property.FindPropertyRelative("id");
        SerializedProperty newAnswer = property.FindPropertyRelative("newAnswer");
        SerializedProperty isCorrect = property.FindPropertyRelative("isCorrect");

        Rect answerRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.PropertyField(answerRect, answer);

        Rect idRect = new Rect(position.x, position.y + (lineHeight + space), position.width, lineHeight);
        EditorGUI.PropertyField(idRect, id);

        int offset = 2;
        if (!string.IsNullOrEmpty(id?.stringValue))
        {
            Rect newAnswerRect = new Rect(position.x, position.y + (lineHeight + space) * offset, position.width, lineHeight);
            EditorGUI.PropertyField(newAnswerRect, newAnswer);
            offset++;
        }

        Rect correctRect = new Rect(position.x, position.y + (lineHeight + space) * offset, position.width, lineHeight);
        EditorGUI.PropertyField(correctRect, isCorrect);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float space = 2f;

        SerializedProperty id = property.FindPropertyRelative("id");
        int lines = 3; // answer, id, isCorrect
        if (!string.IsNullOrEmpty(id?.stringValue))
        {
            lines++; // newAnswer
        }

        return (lineHeight + space) * lines;
    }
}
#endif