using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperChild : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Damage(float damage)
    {
        parent.SendMessage("Damage", damage);
    }
}
