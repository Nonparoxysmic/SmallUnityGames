using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEvent_Int : UnityEvent<int> { }
public class UnityEvent_Int_Int : UnityEvent<int, int> { }
public class UnityEvent_Bool : UnityEvent<bool> { }

public class InputManager : MonoBehaviour
{
    public UnityEvent_Int selectionChanged;
    public UnityEvent_Int_Int selectionActivated;
    public UnityEvent_Bool showSelectionChanged;

    [SerializeField] float repeatDelay;
    [SerializeField] float repeatInterval;
    [SerializeField] GameObject[] columns;

    BoxCollider2D[] columnColliders;
    GameMaster gm;

    bool holdingLeft;
    bool holdingRight;
    DateTime holdLeftStarted;
    DateTime holdRightStarted;
    int currentSelection = -1;
    float holdingLeftDelay;
    float holdingRightDelay;
    Vector3 lastMousePosition;

    void Awake()
    {
        gm = GetComponent<GameMaster>();
        lastMousePosition = Vector3.positiveInfinity;
        repeatInterval = Math.Max(repeatInterval, 0.05f);
        selectionChanged = new UnityEvent_Int();
        selectionActivated = new UnityEvent_Int_Int();
        showSelectionChanged = new UnityEvent_Bool();
        columnColliders = new BoxCollider2D[columns.Length];
        for (int i = 0; i < columns.Length; i++)
        {
            columnColliders[i] = columns[i].GetComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        bool pressedLeft = false;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (holdingLeft)
            {
                if ((DateTime.UtcNow - holdLeftStarted).TotalSeconds > repeatInterval + holdingLeftDelay)
                {
                    pressedLeft = true;
                    holdLeftStarted = DateTime.UtcNow;
                    holdingLeftDelay = 0;
                }
            }
            else
            {
                pressedLeft = true;
                holdLeftStarted = DateTime.UtcNow;
                holdingLeftDelay = repeatDelay;
                holdingLeft = true;
            }
        }
        else holdingLeft = false;

        bool pressedRight = false;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (holdingRight)
            {
                if ((DateTime.UtcNow - holdRightStarted).TotalSeconds > repeatInterval + holdingRightDelay)
                {
                    pressedRight = true;
                    holdRightStarted = DateTime.UtcNow;
                    holdingRightDelay = 0;
                }
            }
            else
            {
                pressedRight = true;
                holdRightStarted = DateTime.UtcNow;
                holdingRightDelay = repeatDelay;
                holdingRight = true;
            }
        }
        else holdingRight = false;

        int horizontal = 0;
        if (pressedLeft) horizontal--;
        if (pressedRight) horizontal++;
        if (holdingLeft && holdingRight) horizontal = 0;
        if (horizontal != 0)
        {
            DirectionPressed(new Vector2Int(horizontal, 0));
        }

        Vector3 mousePos = Input.mousePosition;
        if (mousePos != lastMousePosition)
        {
            MouseMoved(Camera.main.ScreenToWorldPoint(mousePos));
            lastMousePosition = mousePos;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectionActivated(currentSelection);
        }
    }

    void DirectionPressed(Vector2Int direction)
    {
        if (currentSelection < 0)
        {
            if (direction.x > 0) SelectionChanged(0);
            else if (direction.x < 0) SelectionChanged(columns.Length - 1);
        }
        else if (direction.x < 0)
        {
            if (currentSelection > 0)
            {
                SelectionChanged(currentSelection - 1);
            }
            else if (currentSelection == 0)
            {
                SelectionChanged(columns.Length - 1);
            }
        }
        else if (direction.x > 0)
        {
            if (currentSelection < columns.Length - 1)
            {
                SelectionChanged(currentSelection + 1);
            }
            else if (currentSelection == columns.Length - 1)
            {
                SelectionChanged(0);
            }
        }
    }

    void MouseMoved(Vector2 position)
    {
        for (int i = 0; i < columnColliders.Length; i++)
        {
            if (columnColliders[i].bounds.Contains(position))
            {
                if (i != currentSelection)
                {
                    SelectionChanged(i);
                }
                return;
            }
        }
        if (currentSelection >= 0)
        {
            SelectionChanged(-1);
        }
    }

    void SelectionChanged(int value)
    {
        currentSelection = value;
        selectionChanged.Invoke(value);
    }

    void SelectionActivated(int selection)
    {
        gm.SelectionActivated(selection);
    }

    public void ShowSelection(bool doShow)
    {
        showSelectionChanged.Invoke(doShow);
        selectionChanged.Invoke(currentSelection);
    }
}
