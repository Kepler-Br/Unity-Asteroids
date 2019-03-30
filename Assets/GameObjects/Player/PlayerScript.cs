using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public Material mat;
    public float thrustSpeed = 5.0f;
    public float rotationSpeed = 3.0f;
    Vector3[] GeneratePlayerVerticles(float size)
    {
        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(-0.4f, -1.0f, 0.0f)*size;
        vertices[1] = new Vector3(0.0f, 1.0f, 0.0f)*size;
        vertices[2] = new Vector3(0.4f, -1.0f, 0.0f)*size;
        return vertices;
    }
    void SetupLineRenderer(Vector3[] circleVertices)
    {
        LineRenderer lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.positionCount = circleVertices.Length;
        lineRenderer.SetPositions(circleVertices);
    }

    void SetupPolygonCollider2D(Vector3[] circleVertices)
    {
        PolygonCollider2D polygonCollider2D = this.GetComponent<PolygonCollider2D>();
        polygonCollider2D.points = System.Array.ConvertAll(circleVertices, vec3 => new Vector2(vec3.x, vec3.y));
    }
    // Start is called before the first frame update
    void Start()
    { 
        
        
        var objToSpawn = new GameObject("Cool GameObject made from Code");
        objToSpawn.transform.parent = transform;
        LineRenderer lRend = objToSpawn.AddComponent<LineRenderer>();


        Vector3[] circleVertices = GeneratePlayerVerticles(0.5f);
        SetupLineRenderer(circleVertices);
        SetupPolygonCollider2D(circleVertices);
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnPostRender()
    {
        // GL.PushMatrix();
        mat.SetPass(0);
        // GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(new Vector3(-10.4f, -1.0f, 0.0f));
        GL.Vertex(new Vector3(0.0f, 1.0f, 0.0f));
        GL.Vertex(new Vector3(10.4f, -1.0f, 0.0f));
        GL.End();

        // GL.PopMatrix();
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0.0f, 0.0f, rotationSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0.0f, 0.0f, -rotationSpeed);

        if (Input.GetKey(KeyCode.W))
        {
            
            rb.AddForce(this.transform.up*this.thrustSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-this.transform.up*this.thrustSpeed);
        }
    }
}
