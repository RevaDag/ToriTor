using UnityEngine;
using TMPro;

public class WigglyText : MonoBehaviour
{
    private TMP_Text textMeshPro;
    private Mesh mesh;
    private Vector3[] vertices;

    [SerializeField] private float amplitude = 5f; // Amplitude of the wiggle
    [SerializeField] private float frequency = 2f; // Frequency of the wiggle
    [SerializeField] private float phaseShift = 0.5f; // Phase shift for right-to-left animation

    void Awake ()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    void Start ()
    {
        textMeshPro.ForceMeshUpdate();
        mesh = textMeshPro.mesh;
        vertices = mesh.vertices;
    }

    void Update ()
    {
        AnimateWiggle();
    }

    void AnimateWiggle ()
    {
        textMeshPro.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            int vertexIndex = charInfo.vertexIndex;
            vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            // Calculate offset with a phase shift for right-to-left animation
            Vector3 offset = new Vector3(0, Mathf.Sin(Time.time * frequency + (textInfo.characterCount - i) * phaseShift) * amplitude, 0);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}

