using UnityEngine;

public class ToggleOnClick : MonoBehaviour
{
    BoxState bs;
    public GameObject currentLetterSelection;
    SpriteToggler st;

    private void Start()
    {
        currentLetterSelection = GameObject.Find("CurrentLetterSelection");
        st = currentLetterSelection.GetComponent<SpriteToggler>();
        bs = GetComponent<BoxState>();
    }

    public void OnMouseDown()
    {
        if (st.lineHasBeenDrawn)
        {
            return;
        }

        if (bs.CurrentLetter == Letter.Blank)
        {
            bs.SetLetter(st.currentLetter);
            st.ToggleSprite();
        }
    }
}
