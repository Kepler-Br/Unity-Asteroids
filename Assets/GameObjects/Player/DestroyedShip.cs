using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedShip : MonoBehaviour
{
    Transform[] childrens;
    public Quaternion rot;
    // Start is called before the first frame update
    void Start()
    {
        rot = Quaternion.Euler(0.0f, 0.0f, 0.9f);
        childrens = this.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        foreach(Transform children in childrens)
        {
            children.rotation *= rot;
        }
    }
}
