using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keybinds : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI jumpText, dashText, runText, crouchText, parryText, attackText;
    private GameObject currentKey;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Jump Key")) PlayerPrefs.SetString("Jump Key", "Space");
        if (!PlayerPrefs.HasKey("Dash Key")) PlayerPrefs.SetString("Dash Key", "LeftShift");
        if (!PlayerPrefs.HasKey("Run Key")) PlayerPrefs.SetString("Run Key", "LeftControl");
        if (!PlayerPrefs.HasKey("Crouch Key")) PlayerPrefs.SetString("Crouch Key", "S");
        if (!PlayerPrefs.HasKey("Parry Key")) PlayerPrefs.SetString("Parry Key", "E");
        if (!PlayerPrefs.HasKey("Attack Key")) PlayerPrefs.SetString("Attack Key", "Mouse0");

        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key")));
        keys.Add("Dash", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash Key")));
        keys.Add("Run", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Run Key")));
        keys.Add("Crouch", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch Key")));
        keys.Add("Parry", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Parry Key")));
        keys.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack Key")));

        jumpText.SetText(keys["Jump"].ToString());
        dashText.SetText(keys["Dash"].ToString());
        runText.SetText(keys["Run"].ToString());
        crouchText.SetText(keys["Crouch"].ToString());
        parryText.SetText(keys["Parry"].ToString());
        attackText.SetText(keys["Attack"].ToString());

        GameObject.Find("Settings").SetActive(false);
    }

    public void Back()
    {
        keys["Jump"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        keys["Dash"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash Key"));
        keys["Run"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Run Key"));
        keys["Crouch"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch Key"));
        keys["Parry"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Parry Key"));
        keys["Attack"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack Key"));

        PlayerPrefs.SetString("Jump Key", jumpText.GetParsedText());
        PlayerPrefs.SetString("Dash Key", dashText.GetParsedText());
        PlayerPrefs.SetString("Run Key", runText.GetParsedText());
        PlayerPrefs.SetString("Crouch Key", crouchText.GetParsedText());
        PlayerPrefs.SetString("Parry Key", parryText.GetParsedText());
        PlayerPrefs.SetString("Attack Key", attackText.GetParsedText());
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(e.keyCode.ToString());
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }
}
