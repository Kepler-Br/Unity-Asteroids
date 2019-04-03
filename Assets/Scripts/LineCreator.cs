using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineCreator : MonoBehaviour
{
    // [HideInInspector]
    [SerializeField]
    public List<Vector3> lines;
    public bool Loop = false;

    public void CreateLines()
    {
        lines = new List<Vector3> { transform.position + Vector3.zero, transform.position + Vector3.left, transform.position + (Vector3.right + Vector3.up) };
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
