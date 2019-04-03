using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class LineCreator : MonoBehaviour
{
    // [HideInInspector]
    [SerializeField]
    public List<Vector3> lines;
    public bool loop = false;
    public bool autoupdate = false;
    [Range(0.0f, 10.0f)]
    public float lineWidth = 1.0f;

    public void CreateLines()
    {
        lines = new List<Vector3> { transform.position + Vector3.zero, 
                                    transform.position + Vector3.left, 
                                    transform.position + (Vector3.right + Vector3.up) };
    }

    public void UpdateMesh()
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateMesh();
    }

    Mesh CreateMesh()
    {
        Vector3[] vertices = new Vector3[lines.Count*4];
        int[] triangles = new int[lines.Count*2*3];
        int triangleIndex = 0;
        int vertexIndex = 0;
        for (int i = 0; i < lines.Count-1; i++)
        {
            Vector3 pointOne = lines[i];
            Vector3 pointTwo = lines[i + 1];

            Vector3 forward = pointTwo - pointOne;

            Vector3 normal = new Vector3(-forward.y, forward.x, 0.0f);
            normal.Normalize();
            normal *= lineWidth/2.0f;

            vertices[vertexIndex] = pointOne + normal;
            vertices[vertexIndex+1] = pointOne - normal;
            vertices[vertexIndex+2] = pointTwo - normal;
            vertices[vertexIndex+3] = pointTwo + normal;

            triangles[triangleIndex] = vertexIndex;
            triangles[triangleIndex+1] = vertexIndex+2;
            triangles[triangleIndex+2] = vertexIndex+1;

            triangles[triangleIndex+3] = vertexIndex;
            triangles[triangleIndex+4] = vertexIndex+3;
            triangles[triangleIndex+5] = vertexIndex+2;

            vertexIndex+=4;
            triangleIndex+=6;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        return mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
