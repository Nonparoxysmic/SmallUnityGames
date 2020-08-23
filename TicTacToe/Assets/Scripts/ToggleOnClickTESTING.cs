using UnityEngine;

public class ToggleOnClickTESTING : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite xSprite;
    public Sprite oSprite;
    int currentSprite;
    public Animator animatorTest;

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
            if (animatorTest != null) animatorTest.SetInteger("Letter", 1);
        }
        else if (currentSprite == 1)
        {
            sr.sprite = oSprite;
            currentSprite = 2;
            if (animatorTest != null) animatorTest.SetInteger("Letter", 2);
        }
        else
        {
            sr.sprite = null;
            currentSprite = 0;
            if (animatorTest != null) animatorTest.SetInteger("Letter", 0);
        }
    }
}
