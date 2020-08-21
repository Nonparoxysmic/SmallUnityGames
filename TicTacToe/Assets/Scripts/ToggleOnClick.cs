using UnityEngine;

public class ToggleOnClick : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite xSprite;
    public Sprite oSprite;
    int currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = null;
        currentSprite = 0;
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if (currentSprite == 0)
        {
            sr.sprite = xSprite;
            currentSprite = 1;
        }
        else if (currentSprite == 1)
        {
            sr.sprite = oSprite;
            currentSprite = 2;
        }
        else
        {
            sr.sprite = null;
            currentSprite = 0;
        }
    }
}
