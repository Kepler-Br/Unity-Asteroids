using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreenPowerUp : MonoBehaviour
{
    [SerializeField]
    GameObject pickUpSound = null;
    [SerializeField]
    float decayTime = 10.0f;
    float decayTimer = 0.0f;
    Material material = null;
    int mainColorID;
    void Awake()
    {
        decayTimer = decayTime;
        mainColorID = Shader.PropertyToID("_MainColor");
        material = this.GetComponent<Renderer>().material;
        GameEvents.OnPowerupSpawned(this.transform);
    }

    void FixedUpdate()
    {
        decayTimer -= Time.deltaTime;
        float normalizedDecayTime = GetNormalizedDecayTime();
        material.SetColor(mainColorID, new Color(normalizedDecayTime, normalizedDecayTime, normalizedDecayTime, 1.0f));
        if (decayTimer < 0.0f)
            DestroyPowerup();
    }

    void DestroyPowerup()
    {
        GameEvents.OnPowerupDestroyed();
        Destroy(this.gameObject);
    }


    float GetNormalizedDecayTime()
    {
        return decayTimer / decayTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
                    var sound = Instantiate(pickUpSound, transform.position, Quaternion.identity);
        Destroy(sound, 2.0f);
            GameEvents.OnClearScreen();
            DestroyPowerup();
        }
    }
}
