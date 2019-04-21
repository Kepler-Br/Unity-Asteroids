using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreenPowerUp : MonoBehaviour
{
    private GameManager gameManager;
    private float sinTimer = 0.0f;
    public float startYPosition = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();

    }

    void FixedUpdate()
    {
        const float divider = 0.5f;
        const float step = 0.03f;
        Vector3 position = this.transform.position;
        float x = position.x;
        float y = position.y;
        y = startYPosition + Mathf.Sin(sinTimer) / divider;
        x += 0.1f;
        this.transform.position = new Vector3(x, y, 0);
        sinTimer += step;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameEvents.OnPowerupPickup();
            GameEvents.OnClearScreen();
            Destroy(this.gameObject);
        }
    }
}
