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
        // �]�m�C�Ӥ���������
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float space = 2f;  // ���ض������j

        // �]�m�l�ݩ�
        SerializedProperty answer = property.FindPropertyRelative("answer");
        SerializedProperty id = property.FindPropertyRelative("id");
        SerializedProperty newAnswer = property.FindPropertyRelative("newAnswer");

        // ��� 'answer' �M 'id' �ݩ�
        Rect answerRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.PropertyField(answerRect, answer);

        Rect idRect = new Rect(position.x, position.y + lineHeight + space, position.width, lineHeight);
        EditorGUI.PropertyField(idRect, id);

        // �p�G 'id' �����šA��� 'newAnswer' �ݩ�
        if (!string.IsNullOrEmpty(id.stringValue))
        {
            Rect newAnswerRect = new Rect(position.x, position.y + (lineHeight + space) * 2, position.width, lineHeight);
            EditorGUI.PropertyField(newAnswerRect, newAnswer);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // �p�����ݩʪ�����
        SerializedProperty id = property.FindPropertyRelative("id");
        float height = EditorGUIUtility.singleLineHeight * 2 + 2f; // �ܤ���� answer �M id

        if (!string.IsNullOrEmpty(id.stringValue))
        {
            height += EditorGUIUtility.singleLineHeight + 2f; // �p�G�� newAnswer�A�B�~�W�[�䰪��
        }

        return height;
    }
}