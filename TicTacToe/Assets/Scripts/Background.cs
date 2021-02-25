using System;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;

    void Start()
    {
        if (starPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            return;
        }
        for (int i = 0; i < 1000; i++)
        {
            float x = gameObject.transform.position.x + UnityEngine.Random.Range(-25.0f, 25.0f);
            float y = gameObject.transform.position.y + UnityEngine.Random.Range(-5.0f, 5.0f);
            float minScale = 0.0625f;
            float maxScale = 0.625f;
            float scale = Math.Min(Math.Min(UnityEngine.Random.Range(minScale, maxScale), UnityEngine.Random.Range(minScale, maxScale)), UnityEngine.Random.Range(minScale, maxScale));
            Vector3 scaleVector = new Vector3(scale, scale, 1);
            GameObject newStar = Instantiate(starPrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
            newStar.transform.localScale = scaleVector;
            newStar.name = "Star" + i;
        }
    }
}
