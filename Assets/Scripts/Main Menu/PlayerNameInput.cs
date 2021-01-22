using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{

    [Header("UI")]

    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        loadSavedName();
    }

    private void loadSavedName()
    {

        if(!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string savedName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = savedName;

        SetPlayerName(savedName);

    }

    public void SetPlayerName(string savedName)
    {

        if (savedName != null)
        {
            continueButton.interactable = true;
        }

        name = nameInputField.text;
        //Debug.Log(name);
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

}
