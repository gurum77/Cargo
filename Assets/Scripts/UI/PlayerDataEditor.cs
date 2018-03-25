using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDataEditor : MonoBehaviour {

    Dropdown dropDown;
    InputField inputField;
   

	// Use this for initialization
	void Start () {
        dropDown = GetComponentInChildren<Dropdown>();
        inputField = GetComponentInChildren<InputField>();

        List<string> options = new List<string>();
        options.Add(PlayerGameData.CoinsKey);
        options.Add(PlayerGameData.DiamondsKey);
        options.Add(PlayerGameData.FlagModeLevelKey);

        dropDown.ClearOptions();
        dropDown.AddOptions(options);
        

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Define.Scene.Playground);
        }
    }

    string GetCurrentDropdownText()
    {
        return dropDown.options[dropDown.value].text;
    }
    public void OnDropdownChanged()
    {
        // game data를 불러온다.
        inputField.text = PlayerPrefs.GetInt(GetCurrentDropdownText()).ToString();
    }

    public void OnButtonClicked()
    {
        // gameObject data를 쓴다.
        PlayerPrefs.SetInt(GetCurrentDropdownText(), System.Convert.ToInt32(inputField.text));
    }
}
