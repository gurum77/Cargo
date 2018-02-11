using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelectionCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 게임모드를 변경한다.
    void ChangeGameMode(GameModeController.GameMode gameMode)
    {
        PlayerPrefs.SetInt(PlayerGameData.GameModeKey, (int)gameMode);
        SceneManager.LoadScene("Playground");
    }

    // 에너지바 모드 버튼 클릭
    public void OnEnergybarModeButtonClicked()
    {
        ChangeGameMode(GameModeController.GameMode.eEnergyBarMode);        
    }

    // 100M 모드 버튼 클릭
    public void On100MModeButtonClicked()
    {
        ChangeGameMode(GameModeController.GameMode.e100MMode);
    }
}
