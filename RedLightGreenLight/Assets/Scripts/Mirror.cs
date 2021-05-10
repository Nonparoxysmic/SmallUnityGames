using System;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] Sprite mirrorWithEye;

    GameObject player;
    PlayerVision playerVision;
    SpriteRenderer sr;

    Vector3 targetPos;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        targetPos = new Vector3((int)Math.Floor(transform.position.x) + 0.5f, (int)Math.Floor(transform.position.y) - 0.5f, 0);
    }

    void Update()
    {

        if ((player.transform.position - targetPos).magnitude < 0.05 && playerVision.isFacingNorth)
        {
            sr.sprite = mirrorWithEye;

            // count toward the goal here

            Destroy(this);
        }
    }
}
