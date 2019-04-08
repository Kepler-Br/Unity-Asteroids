using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerUpScript : MonoBehaviour
{
    public WeaponType weapon;
    public bool isRandom;
    private float sinTimer = 0.0f;
    public float startYPosition = 0.0f;
    WeaponType[] allowedRandomWeapons = {WeaponType.Chaingun,
                                         WeaponType.RapidFire,
                                         WeaponType.Rockets,};

    void FixedUpdate()
    {
        const float divider = 0.5f;
        const float step = 0.03f;
        Vector3 position = this.transform.position;
        float x = position.x;
        float y = position.y;
        y = startYPosition + Mathf.Sin(sinTimer)/divider;
        x += 0.1f;
        this.transform.position = new Vector3(x, y, 0);
        sinTimer += step;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            WeaponType selectedWeapon;
            if (isRandom)
            {
                int length = allowedRandomWeapons.Length;
                int index = Random.Range(0, length);
                selectedWeapon = allowedRandomWeapons[index];
            }
            else
                selectedWeapon = weapon;
            col.gameObject.SendMessage("SetWeapon", selectedWeapon);
            Destroy(this.gameObject);
        }
    }
}
