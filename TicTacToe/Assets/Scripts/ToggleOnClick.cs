using UnityEngine;

public class ToggleOnClick : MonoBehaviour
{
    public BoxState bs;
    Letter letterToSet = Letter.Blank;

    void OnMouseDown()
    {
        if (bs.CurrentLetter == Letter.Blank) letterToSet = Letter.X;
        else if (bs.CurrentLetter == Letter.X) letterToSet = Letter.O;
        else if (bs.CurrentLetter == Letter.O) letterToSet = Letter.Blank;

        bs.SetLetter(letterToSet);
    }
}
