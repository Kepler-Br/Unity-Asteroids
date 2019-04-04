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
    public bool smoothMesh = true;
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

    Mesh CreateSmoothMesh()
    {
        const int verticesPerLinePoint = 2;
        const int trianglesPerPlane = 2;
        const int verticesPerTriangle = 3;
        Vector3[] vertices = new Vector3[lines.Count * verticesPerLinePoint];
        int[] triangles = new int[lines.Count * trianglesPerPlane * verticesPerTriangle];
        int vertexIndex = 0;

        // Generate first two vertices of line.
        {
            Vector3 pointOne = lines[vertexIndex];
            Vector3 pointTwo = lines[vertexIndex + 1];
            Vector3 normal = pointTwo - pointOne;
            normal = new Vector3(-normal.y, normal.x, 0.0f);
            normal.Normalize();
            normal *= lineWidth / 2.0f;
            vertices[vertexIndex] = pointOne - normal;
            vertexIndex++;
            vertices[vertexIndex] = pointOne + normal;
            vertexIndex++;
        }

        // Vertices.
        for (int i = 0; i < lines.Count - 2; i++)
        {
            Vector3 pointOne = lines[i];
            Vector3 pointTwo = lines[i + 1];
            Vector3 pointThree = lines[i + 2];

            Vector3 normalOne = pointTwo - pointOne;
            Vector3 normalTwo = pointThree - pointTwo;
            // Debug.Log(Vector3.Angle(normalOne, normalTwo));
            // Vector3.c
            normalOne = new Vector3(-normalOne.y, normalOne.x, 0.0f);
            normalTwo = new Vector3(-normalTwo.y, normalTwo.x, 0.0f);
            Vector3 result = normalOne + normalTwo;
            result.Normalize();
            result *= lineWidth / 2.0f;

            vertices[vertexIndex] = pointTwo - result;
            vertexIndex++;
            vertices[vertexIndex] = pointTwo + result;
            vertexIndex++;
        }

        // Triangles.
        vertexIndex = 0;
        int triangleIndex = 0;
        for (int i = 0; i < lines.Count - 1; i++)
        {
            triangles[triangleIndex] = vertexIndex +1;
            triangles[triangleIndex + 1] = vertexIndex + 2;
            triangles[triangleIndex + 2] = vertexIndex;

            triangles[triangleIndex + 3] = vertexIndex + 3;
            triangles[triangleIndex + 4] = vertexIndex + 2;
            triangles[triangleIndex + 5] = vertexIndex + 1;
            
            triangleIndex += 6;
            vertexIndex += 1;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        return mesh;
    }

    Mesh CreateNotShoothMesh()
    {
        const int pointsPerPlane = 4;
        const int trianglesPerPlane = 2;
        const int verticesPerTriangle = 3;
        Vector3[] vertices = new Vector3[lines.Count * pointsPerPlane];
        int[] triangles = new int[lines.Count * trianglesPerPlane * verticesPerTriangle];
        int triangleIndex = 0;
        int vertexIndex = 0;

        for (int i = 0; i < lines.Count - 1; i++)
        {
            Vector3 pointOne = lines[i];
            Vector3 pointTwo = lines[i + 1];

            Vector3 forward = pointTwo - pointOne;

            Vector3 normal = new Vector3(-forward.y, forward.x, 0.0f);
            normal.Normalize();
            normal *= lineWidth / 2.0f;

            vertices[vertexIndex] = pointOne + normal;
            vertices[vertexIndex + 1] = pointOne - normal;
            vertices[vertexIndex + 2] = pointTwo - normal;
            vertices[vertexIndex + 3] = pointTwo + normal;

            triangles[triangleIndex] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 2;
            triangles[triangleIndex + 2] = vertexIndex + 1;

            triangles[triangleIndex + 3] = vertexIndex;
            triangles[triangleIndex + 4] = vertexIndex + 3;
            triangles[triangleIndex + 5] = vertexIndex + 2;

            vertexIndex += 4;
            triangleIndex += 6;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        return mesh;
    }

    Mesh CreateMesh()
    {
        if (smoothMesh)
            return CreateSmoothMesh();
        else
            return CreateNotShoothMesh();
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
