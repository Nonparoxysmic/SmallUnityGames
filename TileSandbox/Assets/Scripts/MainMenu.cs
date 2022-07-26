using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] InputField seedInputField;
    [SerializeField] string gameSceneName;

    MD5 md5;

    void Start()
    {
        if (seedInputField == null)
        {
            this.Error("Input Text reference not set in Inspector.");
            return;
        }
        if (!Utilities.SceneNameIsValid(gameSceneName))
        {
            this.Error($"Invalid scene name \"{gameSceneName}\".");
            return;
        }
        md5 = MD5.Create();
        if (PlayerPrefs.HasKey("seed"))
        {
            seedInputField.text = PlayerPrefs.GetInt("seed").ToString();
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("doRandom", 1);
        PlayerPrefs.SetInt("seed", 0);
        SceneManager.LoadScene(gameSceneName);
    }

    public void ChooseGame()
    {
        PlayerPrefs.SetInt("doRandom", 0);
        string input = seedInputField.text;
        if (!int.TryParse(input, out int seed))
        {
            seed = BitConverter.ToInt32(md5.ComputeHash(Encoding.Unicode.GetBytes(input)), 0);
            PlayerPrefs.SetString("hashedValue", input);
        }
        PlayerPrefs.SetInt("seed", seed);
        SceneManager.LoadScene(gameSceneName);
    }
}
