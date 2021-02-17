using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleVisible : MonoBehaviour
{
    SpriteRenderer sr;

    private void Awake()
    {
        sr = transform.gameObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("ToggleVisible: Missing SpriteRenderer component in object \"" + transform.gameObject.name + "\"");
            sr = transform.gameObject.AddComponent<SpriteRenderer>();
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        sr.enabled = !sr.enabled;
    }
}
