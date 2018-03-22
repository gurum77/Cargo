using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingCanvas : MonoBehaviour {

    public Text modeText;
    public Text playerText;
    public Text mapText;


	// Use this for initialization
	void Start () {
        if (modeText)
            modeText.text = LocalizationText.GetText("Mode");
        if (playerText)
            playerText.text = LocalizationText.GetText("Player");
        if (mapText)
            mapText.text = LocalizationText.GetText("Map");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    // play 버튼 클릭
    public void OnStartButtonClicked()
    {
        GameController.Me.Play();
    }

    // mode 선택 버튼 클릭
    public void OnGameModeSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Me.SaveGameData();

        // player 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.GameModeSelection);
    }

    // map 선택 버튼 클릭
    public void OnMapSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Me.SaveGameData();

        // map 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.MapSelection);
    }

    // player 선택 버튼 클릭
    public void OnPlayerSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Me.SaveGameData();

        // player 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.PlayerSelection);
    }
}
