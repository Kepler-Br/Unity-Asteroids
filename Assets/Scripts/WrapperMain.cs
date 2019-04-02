using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperMain : MonoBehaviour
{
    GameObject[] ghosts = new GameObject[8];

    float screenWidth;
    float screenHeight;


    void OnDestroy()
    {
        foreach (var i in ghosts)
        {
            DestroyImmediate(i);
        }
    }
    void ReceiveScreenGeometry()
    {
        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;
    }

    // TODO: remove simulteniously from childrens. (one function)
    void CreateGhosts()
    {
        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(gameObject, Vector3.zero, Quaternion.identity);

            DestroyImmediate(ghosts[i].GetComponent<WrapperMain>());
            var coms = ghosts[i].GetComponents<MonoBehaviour>();
            foreach (var com in coms)
                Destroy(com);
        }

        // for (int i = 0; i < 8; i++)
            // ghosts[i].transform.parent = gameObject.transform;

    }

    void PositionGhosts()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        var ghostPosition = transform.position;

        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].transform.position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].transform.position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].transform.position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].transform.position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].transform.position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].transform.position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].transform.position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].transform.position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].transform.rotation = transform.rotation;
        }
    }

    void ScreenWrap()
    {
        var newPosition = transform.position;

        if (transform.position.x > screenWidth / 2.0f)
        {
            newPosition.x = -screenWidth / 2.0f;
            transform.position = newPosition;
        }

        if (transform.position.x < -screenWidth / 2.0f)
        {
            newPosition.x = screenWidth / 2.0f;
            transform.position = newPosition;
        }

        if (transform.position.y < -screenHeight / 2.0f)
        {
            newPosition.y = screenHeight / 2.0f;
            transform.position = newPosition;
        }

        if (transform.position.y > screenHeight / 2.0f)
        {
            newPosition.y = -screenHeight / 2.0f;
            transform.position = newPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReceiveScreenGeometry();

        CreateGhosts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        ScreenWrap();
        PositionGhosts();
    }
}
