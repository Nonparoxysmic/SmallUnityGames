using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteToggler : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    [HideInInspector] public Letter currentLetter;
    SpriteRenderer sr;

    [HideInInspector] public bool lineHasBeenDrawn;
    public int numberOfLetters;

    public GameObject bgObject;

    public float delaySeconds = 4.0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (Random.value >= 0.5)
        {
            sr.sprite = xSprite;
            currentLetter = Letter.X;
        }
        else
        {
            sr.sprite = oSprite;
            currentLetter = Letter.O;
        }
        numberOfLetters = 0;
    }

    public void ToggleSprite()
    {
        if (GameObject.Find("Line0") != null)
        {
            lineHasBeenDrawn = true;
            sr.color = Color.green;
            StartCoroutine(WaitAndReset());
            return;
        }

        numberOfLetters++;
        if (numberOfLetters >= 9)
        {
            sr.sprite = null;
            StartCoroutine(WaitAndReset());
            return;
        }

        if (sr.sprite == xSprite)
        {
            sr.sprite = oSprite;
            currentLetter = Letter.O;
        }
        else
        {
            sr.sprite = xSprite;
            currentLetter = Letter.X;
        }
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(0);
    }
}
