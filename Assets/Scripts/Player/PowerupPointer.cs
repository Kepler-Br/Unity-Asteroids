using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class PowerupPointer : MonoBehaviour
{
    // [SerializeField]
    Transform powerup = null;
    MeshRenderer meshRenderer = null;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GameEvents.PowerupSpawned += OnPowerupSpawned;
        GameEvents.PowerupDestroyed += OnPowerupDestroyed;
    }

    void OnPowerupSpawned(Transform powerup)
    {
        this.powerup = powerup;
        meshRenderer.enabled = true;
    }

    void OnPowerupDestroyed()
    {
        powerup = null;
        meshRenderer.enabled = false;
    }

    void Update()
    {
        if (!powerup)
            return;
        float angletotarget = -Mathf.Atan2(powerup.position.x-transform.position.x, powerup.position.y-transform.position.y) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angletotarget);
    }
}
