using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameField : MonoBehaviour
{
    public TMPro.TextMeshProUGUI placeholderNameText;
    public TMPro.TextMeshProUGUI nameText;

    public TMPro.TMP_InputField othertext;

    public void OnEndEdit()
    {
        //The additional checks are due to strange empty inputfield behaviour
        if (string.IsNullOrEmpty(nameText.text) == false && (nameText.text.Length > 1) || (int)nameText.text[0] < 256)
        {
            PlayerPrefs.SetString("PlayerName", nameText.text);
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", "NoName");
        }
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
        if(PlayerPrefs.HasKey("PlayerName"))
        {
            othertext.text = PlayerPrefs.GetString("PlayerName", "NoName");
        } else
        {
            placeholderNameText.text = PlayerPrefs.GetString("PlayerName", "Enter your name here");
            nameText.text = PlayerPrefs.GetString("PlayerName", "NoName");
        }
    }

    private void OnDisable()
    {
        OnEndEdit();
    }
}
