using System;
using UnityEngine;

public class TriangleScript : MonoBehaviour
{
    ToggleTestScript tts;

    void Start()
    {
        GameObject toggleObject = gameObject.transform.parent.gameObject;
        tts = toggleObject.GetComponent<ToggleTestScript>();
    }

    private void OnMouseDown()
    {
        tts.unityEventTest.Invoke(gameObject);
    }
}
