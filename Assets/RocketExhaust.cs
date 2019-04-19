using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExhaust : MonoBehaviour
{

    [SerializeField] private float animationSpeed = 0.05f;
    private MeshRenderer mesh;
    public float animationTimer;

    // Start is called before the first frame update
    void Awake()
    {
        animationTimer = animationSpeed;
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animationTimer -= Time.deltaTime;
        if(animationTimer < 0.0f)
        {
            mesh.enabled = !mesh.enabled;
            animationTimer = animationSpeed;
        }
    }
}
