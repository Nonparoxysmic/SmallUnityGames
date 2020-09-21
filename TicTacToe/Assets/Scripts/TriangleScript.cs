using System;
using UnityEditor.Events;
using UnityEngine;

public class TriangleScript : MonoBehaviour
{
    GameObject toggleObject;
    ToggleTestScript tts;

    void Start()
    {
        toggleObject = gameObject.transform.parent.gameObject;
        tts = toggleObject.GetComponent<ToggleTestScript>();
    }

    private void OnMouseDown()
    {
        //tts.ToggleTest(gameObject);

        tts.unityEventTest.Invoke(gameObject);
    }
}
