using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitDisplayScript : MonoBehaviour
{
    private const int digitCount = 8;
    private const float spaceBetweenDigits = 1.5f;

    private DigitObjectScript[] digits;
    private GameObject digitPrefab;

    void Start()
    {
        digitPrefab = UnityEngine.Resources.Load("DigitObject") as GameObject;
        digits = new DigitObjectScript[digitCount];
        for(int i = 0; i < digitCount; i++)
        {
            Vector3 digitPosition = this.transform.position+Vector3.right*spaceBetweenDigits*i;
            GameObject digitGameObject = Instantiate(digitPrefab, digitPosition, Quaternion.identity);
            DigitObjectScript digitScript = digitGameObject.GetComponent<DigitObjectScript>();
            digits[i] = digitScript;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
