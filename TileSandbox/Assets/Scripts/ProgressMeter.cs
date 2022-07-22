using UnityEngine;

public class ProgressMeter : MonoBehaviour
{
    [SerializeField] float maxWidth;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            this.Error("Missing or unavailable SpriteRenderer.");
            return;
        }
    }

    public void Show(bool doShow)
    {
        sr.enabled = doShow;
        if (!doShow)
        {
            SetHorzScale(1);
        }
    }

    public void SetHorzScale(float horzScaleMultiplier)
    {
        float horzScale = horzScaleMultiplier * maxWidth;
        transform.localScale = new Vector3(horzScale, transform.localScale.y, transform.localScale.z);
    }
}
