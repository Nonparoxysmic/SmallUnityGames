using System;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] GameObject tokenPrefab;
    [SerializeField] GameObject indicatorPrefab;
    [SerializeField] int selectionValue;

    GameMaster gm;
    InputManager inputManager;
    SpriteRenderer sr;
    TokenIndicator indicator;

    bool showSelection;
    Color baseColor;
    int tokenCount;

    void Awake()
    {
        inputManager = GameObject.Find("Main Game").GetComponent<InputManager>();
        gm = GameObject.Find("Main Game").GetComponent<GameMaster>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    void Start()
    {
        GameObject indicatorObject = Instantiate(indicatorPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.5f, -1), Quaternion.identity, gameObject.transform);
        indicator = indicatorObject.GetComponent<TokenIndicator>();
        inputManager.selectionChanged.AddListener(OnSelectionChanged);
        inputManager.selectionActivated.AddListener(OnSelectionActivated);
        inputManager.showSelectionChanged.AddListener(ShowSelectionChanged);
    }

    void OnSelectionChanged(int value)
    {
        if (value == selectionValue && showSelection)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b) * 0.5f + Color.red * 0.5f;
            indicator.Selected(true);
        }
        else
        {
            sr.color = baseColor;
            indicator.Selected(false);
        }
    }

    void OnSelectionActivated(int selection, int player)
    {
        if (selection == selectionValue)
        {
            GameObject tokenObject = Instantiate(tokenPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.5f, -1), Quaternion.identity, gameObject.transform);
            Token tokenScript = tokenObject.GetComponent<Token>();
            tokenScript.SetToken(player);
            tokenScript.SetPhysics(gm.tokenAcceleration, gm.tokenMaxSpeed);
            tokenScript.SetFallPositionY(-2.5f + tokenCount++);
        }
    }

    void ShowSelectionChanged(bool doShow)
    {
        showSelection = doShow;
    }
}
