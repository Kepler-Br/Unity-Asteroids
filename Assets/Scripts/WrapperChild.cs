using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperChild : MonoBehaviour
{
    DamageReceiver damageReceiver = null;
    DamageReceiver parentDamageReceiver = null;

    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        damageReceiver = GetComponent<DamageReceiver>();
        damageReceiver.DamageRecieved += RetranslateDamage;
        damageReceiver.HealthZero += Death;
        parentDamageReceiver = parent.GetComponent<DamageReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Death()
    {
        Debug.Log("Ded");
    }

    void RetranslateDamage(float damage)
    {
        Debug.Log("For fuck sake!");
        parentDamageReceiver.Damage(damage);
    }
}
