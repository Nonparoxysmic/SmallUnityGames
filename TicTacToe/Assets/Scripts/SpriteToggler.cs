using UnityEngine;

public class SpriteToggler : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    public Letter currentLetter;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (Random.value >= 0.5)
        {
            sr.sprite = xSprite;
            currentLetter = Letter.X;
        }
        else
        {
            sr.sprite = oSprite;
            currentLetter = Letter.O;
        }
    }

    public void ToggleSprite()
    {
        if (sr.sprite == xSprite)
        {
            sr.sprite = oSprite;
            currentLetter = Letter.O;
        }
        else
        {
            sr.sprite = xSprite;
            currentLetter = Letter.X;
        }
    }
}
