using UnityEngine;

public class ToggleOnClickTESTING : MonoBehaviour
{
    //SpriteRenderer sr;
    int currentSprite;
    public Animator animatorTest;

    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        //sr.sprite = null;
        currentSprite = 0;
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if (currentSprite == 0)
        {
            currentSprite = 1;
            if (animatorTest != null) animatorTest.SetInteger("Letter", currentSprite);
        }
        else if (currentSprite == 1)
        {
            currentSprite = 2;
            if (animatorTest != null) animatorTest.SetInteger("Letter", currentSprite);
        }
        else
        {
            currentSprite = 0;
            if (animatorTest != null) animatorTest.SetInteger("Letter", currentSprite);
        }
    }
}
