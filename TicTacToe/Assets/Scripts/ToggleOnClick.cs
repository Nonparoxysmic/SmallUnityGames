using UnityEngine;

public class ToggleOnClick : MonoBehaviour
{
    BoxState bs;
    Letter letterToSet;

    private void Start()
    {
        bs = GetComponent<BoxState>();
    }

    void OnMouseDown()
    {
        if (bs.CurrentLetter == Letter.Blank) letterToSet = Letter.X;
        else if (bs.CurrentLetter == Letter.X) letterToSet = Letter.O;
        else if (bs.CurrentLetter == Letter.O) letterToSet = Letter.Blank;

        bs.SetLetter(letterToSet);
    }
}
