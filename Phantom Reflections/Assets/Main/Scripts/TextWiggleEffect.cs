using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWiggleEffect : MonoBehaviour
{
    public float shakeAmount = 2f; // �_�ʴT��
    public float speed = 5f;       // �_�ʳt��

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

            // �����H���_��
            float offsetX = Mathf.Sin(Time.time * speed + i) * shakeAmount;
            float offsetY = Mathf.Cos(Time.time * speed + i) * shakeAmount;
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0);

            // ���ξ_�ʨ�C�ӳ��I
            vertices[vertexIndex + 0] += shakeOffset;
            vertices[vertexIndex + 1] += shakeOffset;
            vertices[vertexIndex + 2] += shakeOffset;
            vertices[vertexIndex + 3] += shakeOffset;
        }

        // ��s Mesh
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
