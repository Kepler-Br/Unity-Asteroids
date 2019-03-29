using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    Vector3[] GeneratePlayerVerticles()
    {
        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(-0.4f, -1.0f, 0.0f);
        vertices[1] = new Vector3(0.0f, 1.0f, 0.0f);
        vertices[2] = new Vector3(0.4f, -1.0f, 0.0f);
        return vertices;
    }
    void SetupLineRenderer(Vector3[] circleVertices)
    {
        LineRenderer lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.loop = true;
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
        Vector3[] circleVertices = GeneratePlayerVerticles();
        SetupLineRenderer(circleVertices);
        SetupPolygonCollider2D(circleVertices);
    }
    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0.0f, 0.0f, 5.0f);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0.0f, 0.0f, -5.0f);

        if (Input.GetKey(KeyCode.W))
        {
            
            rb.AddForce(this.transform.up*4.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-this.transform.up*4.0f);
        }
    }
}
