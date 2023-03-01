using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public event EventHandler TurnEnded;
    public event EventHandler BackButtonClick;
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

    void OnEnable()
    {
        KeyboardInput.KeyDown += OnKeyDown;
    }

    void OnDisable()
    {
        KeyboardInput.KeyDown -= OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
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
        }
    }

    public void OnEndTurnButton()
    {
        TurnEnded?.Invoke(this, EventArgs.Empty);
    }

    public void OnBackButtonClick()
    {
        BackButtonClick?.Invoke(this, EventArgs.Empty);
    }

    public void OnButtonClick(int buttonNumber)
    {
        ButtonClick?.Invoke(this, new IntegerEventArgs(buttonNumber));
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
}
