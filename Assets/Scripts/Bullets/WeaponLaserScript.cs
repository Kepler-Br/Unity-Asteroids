using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaserScript : MonoBehaviour
{
    enum LazerStages
    {
        heatUp,
        damage,
        cooldown,
        death
    }

    public float damage = .01f;

    LazerStages currentStage = LazerStages.heatUp;
    int materialColorID = 0;
    LineCreator lineCreator = null;
    Material material = null;

    float lazerTimer = 0.0f;

    const float heatUpTimer = 0.8f;
    const float damageTimer = 1.0f;
    const float cooldownTimer = 1.5f;

    [SerializeField]
    GameObject sound = null;


    void Awake()
    {

        lineCreator = GetComponent<LineCreator>();
        material = GetComponent<Renderer>().material;
        materialColorID = Shader.PropertyToID("_MainColor");
        PlaySound();
    }

    void PlaySound()
    {
        var soundGameObject = Instantiate(sound);
        Destroy(soundGameObject, 2.0f);
    }

    void NextStage()
    {
        switch (currentStage)
        {
            case LazerStages.heatUp:
                currentStage = LazerStages.damage;
                break;
            case LazerStages.damage:
                currentStage = LazerStages.cooldown;
                break;
            case LazerStages.cooldown:
                currentStage = LazerStages.death;
                break;
        }
    }

    void Update()
    {
        RaycastHit2D rc2d = Physics2D.Raycast(this.transform.position, this.transform.up);
        lineCreator.lines[1] = new Vector3(lineCreator.lines[1].x, rc2d.distance < 0.001 ? 100.0f : rc2d.distance, 0.0f);
        lineCreator.UpdateMesh();
        if (rc2d)
        {
            if (rc2d.collider.gameObject.tag == "Asteroid" && currentStage == LazerStages.damage)
            {
                var damageReciever = rc2d.collider.gameObject.GetComponent<DamageReceiver>();
                damageReciever?.Damage(damage);
            }
        }

        lazerTimer += Time.deltaTime;

        Color resultColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (currentStage == LazerStages.heatUp)
        {
            float normalizedHeatUpTimer = lazerTimer / heatUpTimer;
            resultColor = new Color(normalizedHeatUpTimer, normalizedHeatUpTimer, normalizedHeatUpTimer, 1.0f);
            if (lazerTimer > heatUpTimer)
            {
                NextStage();
                lazerTimer = 0.0f;
            }
        }
        if (currentStage == LazerStages.damage)
        {
            resultColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            if (lazerTimer > damageTimer)
            {
                NextStage();
                lazerTimer = 0.0f;
            }
        }
        if (currentStage == LazerStages.cooldown)
        {
            float normalizedHeatUpTimer = 1.0f - lazerTimer / cooldownTimer;
            resultColor = new Color(normalizedHeatUpTimer, normalizedHeatUpTimer, normalizedHeatUpTimer, 1.0f);
            if (lazerTimer > cooldownTimer)
            {
                NextStage();
                lazerTimer = 0.0f;
            }
        }
        if (currentStage == LazerStages.death)
            Destroy(this.gameObject);

        material.SetColor(materialColorID, resultColor);
    }
}
