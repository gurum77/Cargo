using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
        SceneManager.LoadScene("GameModeSelection");
    }

    // map 선택 버튼 클릭
    public void OnMapSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Me.SaveGameData();

        // map 선택 신으로 변경한다.
        SceneManager.LoadScene("MapSelection");
    }

    // player 선택 버튼 클릭
    public void OnPlayerSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Me.SaveGameData();

        // player 선택 신으로 변경한다.
        SceneManager.LoadScene("PlayerSelection");
    }
}
