using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    public int mirrorsSeen;
    public int totalMirrors;

    EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        totalMirrors = transform.childCount;
    }

    public void MirrorSeen()
    {
        mirrorsSeen++;
        if (mirrorsSeen == totalMirrors)
        {
            enemyManager.onPlayerWin.Invoke();
        }
    }
}
