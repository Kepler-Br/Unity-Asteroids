using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesDisplayScript : MonoBehaviour
{
    Stack<GameObject> liveIcons = new Stack<GameObject>();
    GameObject playerIconPrefab;
    private int lives = 0;
    public int startIconCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerIconPrefab = UnityEngine.Resources.Load("PlayerIcon") as GameObject;
        SetIconCount(startIconCount);
    }

    void AddIcon()
    {
        const float distanceBetweenIcons = 2.0f;
        int liveIconsCount = liveIcons.Count;
        float xPosition = this.transform.position.x + distanceBetweenIcons * liveIconsCount;
        float yPosition = this.transform.position.y;
        Vector3 playerIconPosition = new Vector3(xPosition, yPosition, 0.0f);
        GameObject liveIcon = Instantiate(playerIconPrefab, playerIconPosition, Quaternion.identity);
        liveIcon.transform.parent = this.transform;
        this.liveIcons.Push(liveIcon);
    }

    void RemoveIcon()
    {
        if (liveIcons.Count == 0)
            return;
        lives--;
        GameObject liveIcon = this.liveIcons.Pop();
        Destroy(liveIcon);
    }

    void SetIconCount(int count)
    {
        int liveIconCount = liveIcons.Count;
        if (count > liveIcons.Count)
        {
            for (int i = 0; i < count - liveIconCount; i++)
            {
                this.AddIcon();
            }
        }
        else if (count < liveIcons.Count)
        {
            for (int i = 0; i < liveIconCount - count; i++)
            {
                this.RemoveIcon();
            }
        }
    }
}
