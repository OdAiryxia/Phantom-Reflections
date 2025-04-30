using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Question", menuName = "Exorcism/Question")]
public class ExorcismQuestion : ScriptableObject
{
    [TextArea]
    public string questionText;
    [TextArea(2,10)]
    public string[] protaginistOS;
    public answers[] answers;
}

[System.Serializable]
public class answers
{
    public string answer;
    public string answerOS;
    public bool isCorrect;

    public string id;
    public string newAnswer;
    public string newAnswerOS;
    public bool isNewCorrect;
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(answers))]
public class AnswersDrawer : PropertyDrawer
{
    private static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
    private static Dictionary<string, bool> newAnswerFoldouts = new Dictionary<string, bool>();


    private static GUIStyle titleStyle;
    private static GUIStyle normalStyle;
    private static GUIStyle dimStyle;

    static AnswersDrawer()
    {
        titleStyle = new GUIStyle(EditorStyles.foldout)
        {
            normal = { textColor = new Color(1f, 1f, 1f, 0.85f) },
            fontStyle = FontStyle.Bold
        };

        normalStyle = new GUIStyle(EditorStyles.foldout)
        {
            normal = { textColor = new Color(1f, 1f, 1f, 0.85f) },
            fontStyle = FontStyle.Normal
        };

        dimStyle = new GUIStyle(EditorStyles.foldout)
        {
            normal = { textColor = new Color(0.6f, 0.6f, 0.6f, 0.85f) },
            fontStyle = FontStyle.Normal
        };
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float space = 4f;
        float y = position.y;

        // 欄位參考
        SerializedProperty answer = property.FindPropertyRelative("answer");
        SerializedProperty answerOS = property.FindPropertyRelative("answerOS");
        SerializedProperty id = property.FindPropertyRelative("id");
        SerializedProperty newAnswer = property.FindPropertyRelative("newAnswer");
        SerializedProperty newAnswerOS = property.FindPropertyRelative("newAnswerOS");
        SerializedProperty isCorrect = property.FindPropertyRelative("isCorrect");
        SerializedProperty isNewCorrect = property.FindPropertyRelative("isNewCorrect");

        // 主 Foldout（答案部分）
        string key = property.propertyPath;
        if (!foldouts.ContainsKey(key)) foldouts[key] = true;

        string displayAnswer = string.IsNullOrEmpty(answer.stringValue) ? "(未填寫)" : answer.stringValue;
        foldouts[key] = EditorGUI.Foldout(
            new Rect(position.x, y, position.width, lineHeight),
            foldouts[key],
            $"[答案 {GetElementIndex(property)}] {displayAnswer}",
            true,
            titleStyle
        );
        y += lineHeight + space;

        if (foldouts[key])
        {
            // 顯示主答案的欄位
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), answer, new GUIContent("原始答案"));
            y += lineHeight + space;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), answerOS, new GUIContent("原始答案OS"));
            y += lineHeight + space;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), isCorrect, new GUIContent("是否正確"));
            y += lineHeight + space * 2;

            // 「新的答案」子 Foldout（僅當 id 有值時顯示）
            string subKey = property.propertyPath + "_newAnswer";
            if (!newAnswerFoldouts.ContainsKey(subKey)) newAnswerFoldouts[subKey] = false; // 預設為收合

            // 根據是否輸入ID來改變 Foldout 標題
            string foldoutTitle = string.IsNullOrEmpty(id.stringValue) ? "新的答案" : $"[{id.stringValue}] {newAnswer.stringValue}";
            newAnswerFoldouts[subKey] = EditorGUI.Foldout(
                new Rect(position.x, y, position.width, lineHeight),
                newAnswerFoldouts[subKey],
                foldoutTitle,
                true,
                string.IsNullOrEmpty(id.stringValue) ? dimStyle : normalStyle
            );
            y += lineHeight + space;

            if (newAnswerFoldouts[subKey])
            {
                // 顯示 ID 欄位
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), id, new GUIContent("新的答案 ID"));
                y += lineHeight + space;

                // 只有在 ID 有填寫的情況下才顯示其他欄位
                if (!string.IsNullOrEmpty(id.stringValue))
                {
                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), newAnswer, new GUIContent("新的答案"));
                    y += lineHeight + space;

                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), newAnswerOS, new GUIContent("新的答案OS"));
                    y += lineHeight + space;

                    EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), isNewCorrect, new GUIContent("是否正確"));
                    y += lineHeight + space;
                }
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float space = 4f;
        float height = lineHeight + space; // 主 foldout

        string key = property.propertyPath;
        if (foldouts.ContainsKey(key) && foldouts[key])
        {
            height += (lineHeight + space) * 3; // answer, answerOS, isCorrect
            height += space * 2;

            // 新答案 Foldout
            string subKey = property.propertyPath + "_newAnswer";
            height += lineHeight + space; // newAnswer foldout title

            if (!newAnswerFoldouts.ContainsKey(subKey) || newAnswerFoldouts[subKey])
            {
                height += (lineHeight + space) * 4; // id, newAnswer, newAnswerOS, isNewCorrect
            }
        }

        return height;
    }

    private int GetElementIndex(SerializedProperty property)
    {
        string path = property.propertyPath;
        int start = path.IndexOf("[") + 1;
        int end = path.IndexOf("]");
        if (start >= 0 && end > start)
        {
            string numberStr = path.Substring(start, end - start);
            if (int.TryParse(numberStr, out int index))
            {
                return index + 1; // 顯示從 1 開始
            }
        }
        return -1;
    }
}
#endif