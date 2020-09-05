using UnityEngine;

public class SpriteToggler : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    [HideInInspector] public Letter currentLetter;
    SpriteRenderer sr;

    [HideInInspector] public bool lineHasBeenDrawn;
    int numberOfLetters;

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
        numberOfLetters = 0;
    }

    public void ToggleSprite()
    {
        if (GameObject.Find("Line0") != null)
        {
            lineHasBeenDrawn = true;
            sr.color = Color.green;
            return;
        }

        numberOfLetters++;
        if (numberOfLetters >= 9)
        {
            sr.sprite = null;
            return;
        }

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
