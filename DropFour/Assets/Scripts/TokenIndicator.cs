using System;
using UnityEngine;

public class TokenIndicator : MonoBehaviour
{
    [SerializeField] Sprite redToken;
    [SerializeField] Sprite yellowToken;
    
    public float frequency;
    public float magnitude;

    GameMaster gm;
    SpriteRenderer sr;

    float elapsedSeconds;
    Vector3 basePos;

    void Awake()
    {
        gm = GameObject.Find("Main Game").GetComponent<GameMaster>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        gm.playerColorChanged.AddListener(ChangeCurrentPlayer);
        basePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        elapsedSeconds += Time.deltaTime;
        if (sr.enabled)
        {
            float deltaZ = (float)(magnitude * Math.Abs(Math.Sin(frequency * elapsedSeconds * Math.PI)));
            transform.position = basePos + new Vector3(0, deltaZ, 0);
        }
    }

    public void Selected(bool doSelect)
    {
        if (sr.enabled && !doSelect)
        {
            sr.enabled = false;
        }
        else if (!sr.enabled && doSelect)
        {
            sr.enabled = true;
        }
    }

    void ChangeCurrentPlayer(int player)
    {
        if (player == 0)
        {
            sr.sprite = redToken;
        }
        else sr.sprite = yellowToken;
    }
}
