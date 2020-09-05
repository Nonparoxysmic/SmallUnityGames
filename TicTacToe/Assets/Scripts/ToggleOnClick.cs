using UnityEngine;

public class ToggleOnClick : MonoBehaviour
{
    BoxState bs;
    //Letter letterToSet;
    public GameObject currentLetterSelection;
    SpriteToggler st;

    private void Start()
    {
        currentLetterSelection = GameObject.Find("CurrentLetterSelection");
        st = currentLetterSelection.GetComponent<SpriteToggler>();
        bs = GetComponent<BoxState>();
    }

    void OnMouseDown()
    {
        if (st.lineHasBeenDrawn)
        {
            return;
        }

        //if (bs.CurrentLetter == Letter.Blank) letterToSet = Letter.X;
        //else if (bs.CurrentLetter == Letter.X) letterToSet = Letter.O;
        //else if (bs.CurrentLetter == Letter.O) letterToSet = Letter.Blank;
        if (bs.CurrentLetter == Letter.Blank)
        {
            bs.SetLetter(st.currentLetter);
            st.ToggleSprite();
        }
        else
        {
            //bs.SetLetter(Letter.Blank);
        }

        //bs.SetLetter(letterToSet);
    }
}
