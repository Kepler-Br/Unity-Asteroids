using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreenPowerUp : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameManager.SendMessage("ClearPlayField");
            Destroy(this.gameObject);
        }
    }
}
