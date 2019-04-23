using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamageReceiver))]
[RequireComponent(typeof(LineCreator))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidScript : MonoBehaviour
{
    float radius = 0.0f;

    [SerializeField]
    Rigidbody2D rigidBody = null;
    [SerializeField]
    GameObject soundPrefabOnDestroy = null;
    [SerializeField]
    GameObject asteroidPrefab = null;
    [SerializeField]
    GameObject destroyedParticles = null;
    [SerializeField]
    DamageReceiver damageReceiver = null;
    [SerializeField]
    bool isCrushableToSmallerAsteroids = true;
    int score = 0;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            rigidBody.velocity = -rigidBody.velocity;
        }

    }

    void Start()
    {
        GameEvents.OnAsteroidCreated();
        damageReceiver.HealthZero += OnHealthZero;
    }

    void OnHealthZero()
    {
        OnDeath();
        Destroy(this.gameObject);
    }

    Vector3[] GenerateAsteroidVerticles()
    {
        const int maxPoints = 16;
        const int minPoints = 8;

        int points_number = Random.Range(minPoints, maxPoints);
        Vector3[] vertices = new Vector3[points_number];

        float degree = 0.0f;
        float step = (2 * Mathf.PI) / (points_number);
        for (int i = 0; i < points_number; i++)
        {
            float minRadius = this.radius - this.radius / 2.0f;
            float radius = Random.Range(minRadius, this.radius);
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
        const float maxVelocity = 5.0f;
        const float minVelocity = 0.1f;

        float degree = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = Random.Range(minVelocity, maxVelocity);
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

    static private bool IsCrushableFromRadius(float radius)
    {
        const float minCrushRadius = 3.0f;
        return minCrushRadius < radius ? true : false;
    }

    static private float HitPointsFromRadius(float radius)
    {
        const float multiplyFactor = 40.0f;
        return radius * multiplyFactor;
    }

    public void Initialize(float radius)
    {
        this.radius = radius;
        this.isCrushableToSmallerAsteroids = AsteroidScript.IsCrushableFromRadius(radius);
        damageReceiver.health = AsteroidScript.HitPointsFromRadius(radius);

        Vector3[] asteroidVertices = GenerateAsteroidVerticles();
        SetupLineRenderer(asteroidVertices);
        SetupPolygonCollider2D(asteroidVertices);
        SetupRigidBody2D(asteroidVertices);
    }

    void CreateSmallerAsteroid(float radiusDivider)
    {
        GameObject smallerAsteroid = Instantiate(this.asteroidPrefab, transform.position, Quaternion.identity);
        AsteroidScript asteroidScript = smallerAsteroid.GetComponent<AsteroidScript>();
        asteroidScript.Initialize(this.radius / radiusDivider);
    }

    void OnDeath()
    {
        score = (int)HitPointsFromRadius(this.radius) / 10;
        GameEvents.OnAsteroidDestroyed(score);
        if (this.isCrushableToSmallerAsteroids)
        {
            const int maxSmallerAsteroids = 4;
            int asteroidNum = Random.Range(2, maxSmallerAsteroids);
            for (int i = 0; i < asteroidNum; i++)
                CreateSmallerAsteroid(2);
        }
        
        GameObject sound = Instantiate(soundPrefabOnDestroy);
        Destroy(sound, 3);
        
        var newParticle = Instantiate(destroyedParticles, this.gameObject.transform.position, Quaternion.identity);
        Destroy(newParticle, 5);
    }
}
