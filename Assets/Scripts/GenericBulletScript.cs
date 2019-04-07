using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBulletScript : MonoBehaviour
{
    public float damage = 1.0f;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            col.gameObject.SendMessage("Damage", damage);
            Destroy(this.gameObject);
        }
    }
}
