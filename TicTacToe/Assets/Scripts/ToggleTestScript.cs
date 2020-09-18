using System;
using UnityEngine;

public class ToggleTestScript : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    Letter currentSprite;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentSprite = Letter.X;
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
