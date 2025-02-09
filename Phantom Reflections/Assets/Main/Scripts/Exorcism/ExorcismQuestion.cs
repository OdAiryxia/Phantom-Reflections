using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Question", menuName = "Exorcism/Question")]
public class ExorcismQuestion : ScriptableObject
{
    [TextArea]
    public string questionText;
    public answers[] answers;
    public int correctAnswerIndex;
}

[System.Serializable]
public class answers
{
    public string answer;
    public string id;
    public string newAnswer;
}

[CustomPropertyDrawer(typeof(answers))]
public class AnswersDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 設置每個元素的高度
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float space = 2f;  // 項目間的間隔

        // 設置子屬性
        SerializedProperty answer = property.FindPropertyRelative("answer");
        SerializedProperty id = property.FindPropertyRelative("id");
        SerializedProperty newAnswer = property.FindPropertyRelative("newAnswer");

        // 顯示 'answer' 和 'id' 屬性
        Rect answerRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.PropertyField(answerRect, answer);

        Rect idRect = new Rect(position.x, position.y + lineHeight + space, position.width, lineHeight);
        EditorGUI.PropertyField(idRect, id);

        // 如果 'id' 不為空，顯示 'newAnswer' 屬性
        if (!string.IsNullOrEmpty(id.stringValue))
        {
            Rect newAnswerRect = new Rect(position.x, position.y + (lineHeight + space) * 2, position.width, lineHeight);
            EditorGUI.PropertyField(newAnswerRect, newAnswer);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 計算整個屬性的高度
        SerializedProperty id = property.FindPropertyRelative("id");
        float height = EditorGUIUtility.singleLineHeight * 2 + 2f; // 至少顯示 answer 和 id

        if (!string.IsNullOrEmpty(id.stringValue))
        {
            height += EditorGUIUtility.singleLineHeight + 2f; // 如果有 newAnswer，額外增加其高度
        }

        return height;
    }
}