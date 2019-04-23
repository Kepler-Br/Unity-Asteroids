using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    GameObject target = null;
    [SerializeField]
    Vector3 maximumDeviation;

    void FixedUpdate()
    {
        const float z = -30.0f;
        float targetX = target.transform.position.x;
        float targetY = target.transform.position.y;
        float cameraX = transform.position.x;
        float cameraY = transform.position.y;
        float deltaX = targetX - cameraX;
        float deltaY = targetY - cameraY;
        if (deltaX > maximumDeviation.x || deltaX < -maximumDeviation.x || deltaY > maximumDeviation.y || deltaY < -maximumDeviation.y)
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(targetX, targetY, z), 0.1f);
        //     this.transform.position = new Vector3(targetX - maximumDeviation.x, cameraY, z);
        // if (deltaX < -maximumDeviation.x)
        //     this.transform.position = new Vector3(targetX + maximumDeviation.x, cameraY, z);
        // if (deltaY > maximumDeviation.y)
        //     this.transform.position = new Vector3(cameraX, targetY - maximumDeviation.y, z);
        // if (deltaY < -maximumDeviation.y)
        //     this.transform.position = new Vector3(cameraX, targetY + maximumDeviation.y, z);

        // this.transform.position = new Vector3(x, y, z);
    }
}
