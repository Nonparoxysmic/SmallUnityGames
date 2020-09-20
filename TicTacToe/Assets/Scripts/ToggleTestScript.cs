using System;
using UnityEngine;
using UnityEngine.Events;

public class ToggleTestScript : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    Letter currentSprite;
    SpriteRenderer sr;

    public UnityEvent unityEventTest;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentSprite = Letter.X;

        unityEventTest.AddListener(ToggleTest2);
    }

    public void ToggleTest(GameObject toggler)
    {
        if (currentSprite == Letter.X)
        {
            sr.sprite = oSprite;
            currentSprite = Letter.O;
        }
        else if (currentSprite == Letter.O)
        {
            sr.sprite = xSprite;
            currentSprite = Letter.X;
        }
        Debug.Log("Toggled by " + toggler.name);
    }

    void ToggleTest2()
    {
        ToggleTest(gameObject);
    }
}
