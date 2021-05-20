using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] SpawnerType spawnerType;
    [SerializeField] int spriteNumber;
    [SerializeField] int detectionRange = 999;
    [SerializeField] int moveDelay = 1;
    [SerializeField] int wakeCountdown = 0;

    [SerializeField] Sprite[] sprites;

    Color color;
    GameObject enemyManager;
    GameObject obstaclesObject;
    GameObject parentObject;
    Sprite spriteBase;
    Sprite spriteColor;
    SpriteRenderer sr;

    GameObject thing;
    GameObject thingColor;

    void Awake()
    {
        enemyManager = GameObject.Find("EnemyManager");
        obstaclesObject = GameObject.Find("Obstacles");
        if (spawnerType == SpawnerType.Enemy) parentObject = enemyManager;
        else parentObject = obstaclesObject;

        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        if (spriteNumber < 0 || spriteNumber > 3)
        {
            spriteNumber = UnityEngine.Random.Range(0, 4);
        }
        spriteBase = sprites[spriteNumber];
        spriteColor = sprites[spriteNumber + 4];
    }

    void Start()
    {
        thing = new GameObject();
        thing.transform.position = transform.position;
        thing.transform.parent = parentObject.transform;
        SpriteRenderer thingSprite = thing.AddComponent<SpriteRenderer>();
        thingSprite.sprite = spriteBase;

        thingColor = new GameObject();
        thingColor.transform.position = new Vector3(thing.transform.position.x, thing.transform.position.y, thing.transform.position.z - 0.5f);
        thingColor.transform.parent = thing.transform;
        SpriteRenderer thingColorSprite = thingColor.AddComponent<SpriteRenderer>();
        thingColorSprite.sprite = spriteColor;
        thingColorSprite.color = color;
        thingColor.name = "Color";

        if (spawnerType == SpawnerType.Enemy)
        {
            thing.name = "Enemy";
            EnemyBehavior eb = thing.AddComponent<EnemyBehavior>();
            eb.detectionRange = detectionRange;
            eb.moveDelay = moveDelay;
            eb.wakeCountdown = wakeCountdown;
        }
        else
        {
            thing.name = "Obstacle";
            thing.AddComponent<Obstacle>();
        }
        Destroy(gameObject);
    }
}

enum SpawnerType
{
    Enemy,
    Obstacle
}
