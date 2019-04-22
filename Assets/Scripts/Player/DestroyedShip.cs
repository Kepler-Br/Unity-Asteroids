using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedShip : MonoBehaviour
{
    Transform[] childrens;
    Vector3[] directions;
    private Transform target;
    float coeff = 0.0f;

    public bool rebuildAnimation = true;
    public float explosionStrength = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        int childrenCount = this.transform.childCount;
        childrens = new Transform[childrenCount];
        directions = new Vector3[childrenCount];
        for (int i = 0; i < childrenCount; i++)
        {
            childrens[i] = this.transform.GetChild(i);
        }
        for (int i = 0; i < childrenCount; i++)
        {
            float degree = Mathf.PI * 2 / childrenCount * i;
            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
            direction.x = Mathf.Cos(degree);
            direction.y = Mathf.Sin(degree);
            directions[i] = direction;
        }

    }

    void AnimationWithRebuild()
    {
        coeff += coeff > 1.0f ? 0.0f : 0.01f;
        for (int i = 0; i < childrens.Length; i++)
        {
            Vector3 explosionDirection = directions[i] * (1.0f - coeff) * explosionStrength;
            Vector3 distanceToTarget = target.position - childrens[i].position;
            Vector3 directionToTarget = distanceToTarget.normalized * (coeff);
            Vector3 resultDirection = explosionDirection + directionToTarget;

            if (coeff > 0.5f)
                childrens[i].position += resultDirection * Vector3.Magnitude(distanceToTarget);
            else
                childrens[i].position += resultDirection;

            if (coeff > 0.5f)
            {
                childrens[i].rotation = Quaternion.Lerp(childrens[i].rotation, target.rotation, 0.9f);

            }
            else
            {
                childrens[i].rotation *= Quaternion.Euler(0.0f, 0.0f, 20.0f * (1.0f - coeff));

            }

        }
    }

    void AnimationWithoutRebuild()
    {
        for (int i = 0; i < childrens.Length; i++)
        {
            Vector3 explosionDirection = directions[i] * explosionStrength;
            childrens[i].position += explosionDirection;
            childrens[i].rotation *= Quaternion.Euler(0.0f, 0.0f, 20.0f);
        }
    }


    void FixedUpdate()
    {
        if (rebuildAnimation)
            AnimationWithRebuild();
        else
            AnimationWithoutRebuild();
        if(coeff >= 1.0f)
        {
            target.SendMessage("OnDeathAnimationEnd");
            GameObject.Destroy(this.gameObject);
        }
    }
}
