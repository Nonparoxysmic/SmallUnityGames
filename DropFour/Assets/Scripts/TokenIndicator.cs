using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenIndicator : MonoBehaviour
{
    bool isVisible;

    public void Selected(bool doSelect)
    {
        if (isVisible && !doSelect)
        {
            Debug.Log("Indicator deselected.");
            isVisible = false;
        }
        else if (!isVisible && doSelect)
        {
            Debug.Log("Indicator selected.");
            isVisible = true;
        }
    }
}
