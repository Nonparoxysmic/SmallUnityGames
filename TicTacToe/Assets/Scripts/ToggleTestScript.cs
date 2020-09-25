using System;
using UnityEngine;
using UnityEngine.Events;

public class MyEventTest : UnityEvent<GameObject> { }

public class ToggleTestScript : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    Letter currentSprite;
    SpriteRenderer sr;

    public MyEventTest toggleEvent;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentSprite = Letter.X;
        if (toggleEvent == null)
        {
            toggleEvent = new MyEventTest();
        }
        toggleEvent.AddListener(ToggleTest);
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
}
