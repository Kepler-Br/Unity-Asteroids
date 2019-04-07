using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBulletScript : MonoBehaviour
{
    public float damage = 1.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            col.gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}
