using UnityEngine;
using UnityEngine.UI;

public class ToggleVisible : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = transform.gameObject.GetComponent<Image>();
    }

    public void OnClick()
    {
        if (image.color.a < 0.5f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }

    public void Hide()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}
