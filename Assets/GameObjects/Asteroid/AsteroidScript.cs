using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineCreator))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidScript : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float maxRadius = 5.0f;
    public float minRadius = 4.0f;
    public int canBeDestroyedTimes = 1;
    private float hitpoints = 100.0f;

    void Damage(float hitpoints)
    {
        this.hitpoints -= hitpoints;
    }

    Vector3[] GenerateAsteroidVerticles()
    {
        const int MAX_POINTS = 16;
        const int MIN_POINTS = 8;

        int points_number = Random.Range(MIN_POINTS, MAX_POINTS);
        Vector3[] vertices = new Vector3[points_number];

        float degree = 0.0f;
        float step = (2*Mathf.PI)/(points_number);
        for (int i = 0; i < points_number; i++)
        {
            float radius = Random.Range(minRadius, maxRadius);
            Vector3 vertex = new Vector3(0.0f, 0.0f, 0.0f);
            vertex.x = radius * Mathf.Cos(degree);
            vertex.y = radius * Mathf.Sin(degree);
            vertices[i] = vertex;
            degree += step;
        }

        return vertices;
    }

    void SetupLineRenderer(Vector3[] circleVertices)
    {
        LineCreator lineCreator = this.GetComponent<LineCreator>();
        lineCreator.SetPoints(circleVertices);
        lineCreator.AddPoint(circleVertices[0]);
    }

    void SetupPolygonCollider2D(Vector3[] circleVertices)
    {
        PolygonCollider2D polygonCollider2D = this.GetComponent<PolygonCollider2D>();
        polygonCollider2D.points = System.Array.ConvertAll(circleVertices, vec3 => new Vector2(vec3.x, vec3.y));
    }

    Vector2 GenerateRandomVelocity()
    {
        const float MAX_VELOCITY = 5.0f;
        const float MIN_VELOCITY = 0.1f;

        float degree = Random.Range(0.0f, Mathf.PI*2.0f);
        float radius = Random.Range(MIN_VELOCITY, MAX_VELOCITY);
        Vector2 velocity = new Vector2();
        velocity.x = radius * Mathf.Cos(degree);
        velocity.y = radius * Mathf.Sin(degree);
        return velocity;
    }

    void SetupRigidBody2D(Vector3[] circleVertices)
    {
        Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();
        Vector2 velocity = GenerateRandomVelocity();
        rigidbody2D.velocity = velocity;
    }

    public void Initialize(float maxRadius, float minRadius, int canBeDestroyedTimes)
    {
        this.maxRadius = maxRadius;
        this.minRadius = minRadius;
        this.canBeDestroyedTimes = canBeDestroyedTimes;

        Vector3[] circleVertices = GenerateAsteroidVerticles();
        SetupLineRenderer(circleVertices);
        SetupPolygonCollider2D(circleVertices);
        SetupRigidBody2D(circleVertices);
    }

    void CreateSmallerAsteroid()
    {
        GameObject smallerAsteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        AsteroidScript asteroidScript = smallerAsteroid.GetComponent<AsteroidScript>();
        asteroidScript.Initialize(maxRadius/2.0f, minRadius/2.0f, canBeDestroyedTimes-1);
    }

    // Update is called once per frame
    void LateUpdate()
    {
                if(this.hitpoints < 0.0f)
        {
            if(canBeDestroyedTimes > 0)
            {
                const int maxSmallerAsteroids = 4;
                int asteroidNum = Random.Range(2, maxSmallerAsteroids);
                for(int i = 0; i < asteroidNum; i++)
                    CreateSmallerAsteroid();
            }
            Destroy(this.gameObject);
        }
    }
}
