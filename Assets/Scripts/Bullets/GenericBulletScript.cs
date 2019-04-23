using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBulletScript : MonoBehaviour
{
    [SerializeField]
    float damage = 1.0f;
    [SerializeField]
    bool destroyOnAsteroidContact = true;
    [SerializeField]
    GameObject sound = null;

    void Awake()
    {
        PlaySound();
    }

    void PlaySound()
    {
        var soundGameObject = Instantiate(sound);
        Destroy(soundGameObject, 2.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Asteroid" || col.gameObject.tag == "Wall")
        {
            var damageReciever = col.gameObject.GetComponent<DamageReceiver>();
            damageReciever?.Damage(damage);
            if (destroyOnAsteroidContact)
                Destroy(this.gameObject);
        }
    }
}
