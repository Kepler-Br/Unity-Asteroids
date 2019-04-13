using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(LineCreator)]
public class DigitObjectScript : MonoBehaviour
{
    private LineCreator lineCreator;

    static Vector3[] GetDigit(int digit)
    {
        return new Vector3[]{new Vector3(-0.5f, -1.0f, 0.0f),
                             new Vector3( 0.5f, -1.0f, 0.0f),
                             new Vector3( 0.5f,  1.0f, 0.0f),
                             new Vector3(-0.5f,  1.0f, 0.0f),
                             new Vector3(-0.5f, -1.0f, 0.0f)};
    }

    void Start()
    {
        lineCreator = this.GetComponent<LineCreator>();
        this.SetDigit(0);
    }


    void Update()
    {

    }

    public void SetDigit(int digit)
    {
        lineCreator.SetPoints(GetDigit(digit));
    }
}
