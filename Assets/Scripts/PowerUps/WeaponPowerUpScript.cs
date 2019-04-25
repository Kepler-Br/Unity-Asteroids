using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerUpScript : MonoBehaviour
{
    [SerializeField]
    float decayTime = 10.0f;
    float decayTimer = 0.0f;
    [SerializeField]
    WeaponType weapon;
    [SerializeField]
    GameObject pickUpSound = null;
    [SerializeField]
    bool isRandom = false;

    WeaponType[] allowedRandomWeapons = {WeaponType.Chaingun,
                                         WeaponType.RapidFire,
                                         WeaponType.Rockets,
                                         WeaponType.Lazer,
                                         WeaponType.Shotgun,
                                         WeaponType.Stinger,};

    Material material = null;
    int mainColorID = 0;

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
            WeaponType selectedWeapon;
            if (isRandom)
            {
                int length = allowedRandomWeapons.Length;
                int index = Random.Range(0, length);
                selectedWeapon = allowedRandomWeapons[index];
            }
            else
                selectedWeapon = weapon;
            GameEvents.OnWeaponChanged(selectedWeapon);
            DestroyPowerup();
        }
    }
}
