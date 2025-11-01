using System;
using TMPro;
using UnityEngine;

public class CurvedText : MonoBehaviour
{
    [SerializeField, Range(0, 360)] private float arcLength = 180f;
    [SerializeField, Range(0, 360)] private float startAngle = 180f;
    [SerializeField] private float radius = 200f;

    [SerializeField] private bool clockWise = true;

    private TMP_Text textToCurve;

    private bool isInitialized = false;

    private void Awake()
    {
        textToCurve = GetComponent<TMP_Text>();

        isInitialized = true;
    }

    private void Start() => CurveText();
    private void OnEnable() => CurveText();
    private void OnRectTransformDimensionsChange()
    {
        if (!isInitialized)
        {
            return;
        }
        
        CurveText();
    }
    
    private void CurveText()
    {
        textToCurve.ForceMeshUpdate();
        var textInfo = textToCurve.textInfo;
        int totalCharacters = textInfo.characterCount;

        if (totalCharacters == 0)
        {
            return;
        }
        
        int visibleCount = 0;
        for (int i = 0; i < totalCharacters; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                visibleCount++;
            }
        }

        if (visibleCount == 0) return;
        if (visibleCount == 1)
        {
            return;
        }

        float angleStep = (visibleCount > 1) ? (arcLength / (visibleCount - 1)) : 0f;
        if (clockWise)
        {
            angleStep = -angleStep;
        }

        int visibleSeen = 0;
        for (int i = 0; i < totalCharacters; i++)
        {
            var characterInfo = textInfo.characterInfo[i];

            if (!characterInfo.isVisible)
            {
                continue;
            }


            int martialIndex = characterInfo.materialReferenceIndex;
            int vertexIndex = characterInfo.vertexIndex;

            Vector3[] verts = textInfo.meshInfo[martialIndex].vertices;
            Vector3 charMid = (verts[vertexIndex + 0] + verts[vertexIndex + 2]) * 0.5f;

            float currentAngle = startAngle + visibleSeen * angleStep;
            float angleRad = currentAngle * Mathf.Deg2Rad;;

            Vector3 newCenter = new Vector3(MathF.Cos(angleRad), MathF.Sin(angleRad), 0f) * radius;

            Quaternion rot = Quaternion.Euler(0f, 0f, currentAngle - 90f);

            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = verts[vertexIndex + j];
                Vector3 offset = orig - charMid;
                Vector3 rotated = rot * offset;
                verts[vertexIndex + j] = newCenter + rotated;
            }

            visibleSeen++;
        }
        
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textToCurve.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
