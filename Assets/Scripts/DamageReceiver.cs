using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageReceiver : MonoBehaviour
{
    public Action<float> DamageRecieved;
    public Action HealthZero;

    [SerializeField]
    public float health = 0;

    public void Damage(float damage)
    {
        DamageRecieved?.Invoke(damage);
        health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            HealthZero?.Invoke();
    }
}
