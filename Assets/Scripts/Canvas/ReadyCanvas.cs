using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Controller;

public class ReadyCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text gameModeText;
    public Text gameModeSubText;
    public GameObject exitCanvasItems;
    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // display score
        DisplayScore();

        // display best
        DisplayBestScore();

        // display game mode
        DisplayGameMode();

        if (Input.GetKeyDown(KeyCode.Escape) && exitCanvasItems)
        {
            exitCanvasItems.SetActive(exitCanvasItems.activeSelf ? false : true);
        }
	}

    // 게임 모드를 표시한다.
    void DisplayGameMode()
    {
        if (gameModeText)
            gameModeText.text = GameController.Instance.gameModeController.GetCurGameModeDisplayName();

        if(gameModeSubText)
        {
            if(GameController.Instance.gameModeController.GetCurGameMode() == GameModeController.GameMode.eFlagMode)
            {
                gameModeSubText.text = StringMaker.GetFlagModeLevelString();
            }
            else
            {
                gameModeSubText.text = "";
            }
        }
    }

    public void OnStartButtonClicked()
    {
        GameController.Instance.Play();
    }


    // 최고점수 출력
    void DisplayBestScore()
    {
        if (bestScoreText)
        {
            // 모드별로 다르게 표시한다.
            GameModeController.GameMode curGameMode = GameController.Instance.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode)
                bestScoreText.text = LocalizationText.GetText("Best ") + GameController.Instance.Player.GameData.EnergyBarModeBestScore.ToString();
            else if (curGameMode == GameModeController.GameMode.e100MMode)
                bestScoreText.text = LocalizationText.GetText("Best ") + GameMode_100M.TimeToString(GameController.Instance.Player.GameData.HundredMBestTime);
        }
    }


    // 점수 출력
    void DisplayScore()
    {
        if(scoreText)
            scoreText.text = GameController.Instance.Player.Score.ToString();
    }


    // mode 선택 버튼 클릭
    public void OnGameModeSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Instance.SaveGameData();

        // player 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.GameModeSelection);
    }

    // map 선택 버튼 클릭
    public void OnMapSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Instance.SaveGameData();

        // map 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.MapSelection);
    }

    // player 선택 버튼 클릭
    public void OnPlayerSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Instance.SaveGameData();
        
        // player 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.PlayerSelection);
    }

    // 리더보드 버튼 클릭
    public void OnLeaderBoardButtonClicked()
    {
        

    }

    public void OnGameDataEditorButtonClicked()
    {
        // game data를 저장한다.
        GameController.Instance.SaveGameData();

        // player 선택 신으로 변경한다.
        SceneManager.LoadScene(Define.Scene.PlayerDataEditor);
    }
}
