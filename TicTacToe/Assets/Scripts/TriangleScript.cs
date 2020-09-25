using System;
using UnityEngine;

public class TriangleScript : MonoBehaviour
{
    ToggleTestScript tts;

    private void Start()
    {
        tts = gameObject.transform.parent.gameObject.GetComponent<ToggleTestScript>();
    }

    private void OnMouseDown()
    {
        tts.toggleEvent.Invoke(gameObject);
    }
}
