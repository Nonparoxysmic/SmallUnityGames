using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    public int mirrorsSeen;

    EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    public void MirrorSeen()
    {
        mirrorsSeen++;
        if (mirrorsSeen == transform.childCount)
        {
            enemyManager.onPlayerWin.Invoke();
        }
    }
}
