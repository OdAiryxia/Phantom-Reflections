using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWiggleEffect : MonoBehaviour
{
    public float shakeAmount = 2f; // 震動幅度
    public float speed = 5f;       // 震動速度

    private TMP_Text textMeshPro;
    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;
        StoreOriginalVertices();
    }

    void Update()
    {
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

            // 產生隨機震動
            float offsetX = Mathf.Sin(Time.time * speed + i) * shakeAmount;
            float offsetY = Mathf.Cos(Time.time * speed + i) * shakeAmount;
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0);

            // 應用震動到每個頂點
            vertices[vertexIndex + 0] += shakeOffset;
            vertices[vertexIndex + 1] += shakeOffset;
            vertices[vertexIndex + 2] += shakeOffset;
            vertices[vertexIndex + 3] += shakeOffset;
        }

        // 更新 Mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    void StoreOriginalVertices()
    {
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = new Vector3[textInfo.meshInfo[i].vertices.Length];
            System.Array.Copy(textInfo.meshInfo[i].vertices, originalVertices[i], textInfo.meshInfo[i].vertices.Length);
        }
    }
}
