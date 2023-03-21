using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public event EventHandler TurnEnded;
    public event EventHandler BackButtonClick;
    public event EventHandler EndBattleButtonClick;
    public event EventHandler<IntegerEventArgs> ButtonClick;

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button ButtonBack;
    public Text Button1Text;
    public Text Button2Text;
    public Text Button3Text;
    public Text Button4Text;
    public Text ButtonBackText;

    public Image ActiveUnitImage;
    public Text ActiveUnitName;
    public Text ActiveUnitStats;

    public GameObject EscapeMenuPanel;
    public bool IsPaused => EscapeMenuPanel.activeSelf;

    public Tilemap HighlightTilemap { get; set; }

    Tile _highlightTile;
    UnitController _activeUnit;

    void OnEnable()
    {
        if (EscapeMenuPanel == null)
        {
            this.Error($"{nameof(EscapeMenuPanel)} reference not set in the Inspector.");
            return;
        }
        EscapeMenuPanel.SetActive(false);
        _highlightTile = ScriptableObject.CreateInstance<Tile>();
        _highlightTile.sprite = AssetLibrary.CreateSquareSprite(4, Color.white);
        KeyboardInput.KeyDown += OnKeyDown;
    }

    void OnDisable()
    {
        KeyboardInput.KeyDown -= OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (IsPaused && e.KeyCode != KeyCode.Escape)
        {
            return;
        }
        switch (e.KeyCode)
        {
            case KeyCode.Return:
                TurnEnded?.Invoke(this, EventArgs.Empty);
                break;
            case KeyCode.Alpha1:
            case KeyCode.Alpha2:
            case KeyCode.Alpha3:
            case KeyCode.Alpha4:
                int buttonNumber = (int)e.KeyCode - 48;
                ButtonClick?.Invoke(this, new IntegerEventArgs(buttonNumber));
                break;
            case KeyCode.Backspace:
                BackButtonClick?.Invoke(this, EventArgs.Empty);
                break;
            case KeyCode.Escape:
                ToggleEscapeMenu();
                break;
        }
    }

    public void OnEndTurnButton()
    {
        if (!IsPaused)
        {
            TurnEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnBackButtonClick()
    {
        if (!IsPaused)
        {
            BackButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnButtonClick(int buttonNumber)
    {
        if (!IsPaused)
        {
            ButtonClick?.Invoke(this, new IntegerEventArgs(buttonNumber));
        }
    }

    public void OnFocusActiveUnitButton()
    {
        if (_activeUnit != null)
        {
            CameraController.LookAtUnit(_activeUnit);
        }
    }

    public void OnEndBattleButtonClick()
    {
        EndBattleButtonClick?.Invoke(this, EventArgs.Empty);
    }

    public void SetBackButtonInteractable(bool interactable)
    {
        ButtonBack.interactable = interactable;
        ButtonBackText.text = interactable ? "Back" : string.Empty;
    }

    public void SetButtons(string text1 = "", string text2 = "", string text3 = "", string text4 = "")
    {
        Button1.interactable = text1 != "";
        Button1Text.text = text1;
        Button2.interactable = text2 != "";
        Button2Text.text = text2;
        Button3.interactable = text3 != "";
        Button3Text.text = text3;
        Button4.interactable = text4 != "";
        Button4Text.text = text4;
    }

    public void SetButtonInteractable(int buttonNumber, bool interactable)
    {
        switch (buttonNumber)
        {
            case 1:
                Button1.interactable = interactable;
                break;
            case 2:
                Button2.interactable = interactable;
                break;
            case 3:
                Button3.interactable = interactable;
                break;
            case 4:
                Button4.interactable = interactable;
                break;
        }
    }

    public void ClearButtons()
    {
        Button1.interactable = false;
        Button1Text.text = "";
        Button2.interactable = false;
        Button2Text.text = "";
        Button3.interactable = false;
        Button3Text.text = "";
        Button4.interactable = false;
        Button4Text.text = "";
    }

    public void SetActiveUnit(UnitController unit)
    {
        _activeUnit = unit;
        ActiveUnitImage.sprite = unit.CreatureStats.Species.BaseSprite;
        ActiveUnitImage.color = unit.CreatureStats.Species.BaseColor;
        ActiveUnitName.text = unit.CreatureStats.IndividualName;
        ActiveUnitStats.text = $"{unit.CurrentHP} / {unit.CreatureStats.MaximumHP} HP";
    }

    void ToggleEscapeMenu()
    {
        EscapeMenuPanel.SetActive(!EscapeMenuPanel.activeSelf);
    }

    public void HighlightCells(List<Vector2Int> cellCoords, Color color)
    {
        if (HighlightTilemap == null)
        {
            return;
        }
        _highlightTile.color = color;
        foreach (Vector2Int coord in cellCoords)
        {
            HighlightTilemap.SetTile((Vector3Int)coord, _highlightTile);
        }
    }

    public void ClearCellHighlights()
    {
        if (HighlightTilemap != null)
        {
            HighlightTilemap.ClearAllTiles();
        }
    }
}
