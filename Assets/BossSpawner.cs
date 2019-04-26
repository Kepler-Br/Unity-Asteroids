using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] _bossList;
    [SerializeField]
    SmoothBlinkingText warningTitle = null;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.AllAsteroidsDestroyed += OnAllAsteroidsDestroyed;
        warningTitle.EndBlinking += OnWarningTitleEndBlinking;
        // warningTitle.Restart();
    }

    void OnAllAsteroidsDestroyed()
    {
        warningTitle.enabled = true;
        warningTitle.StartBlink();
    }

    void OnWarningTitleEndBlinking()
    {
        int bossIndex = Random.Range(0, _bossList.Length);
        Instantiate(_bossList[bossIndex], Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
