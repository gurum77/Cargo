using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text coinsText;
    public Text gameModeText;
    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // display score
        DisplayScore();

        // display best
        DisplayBestScore();

        // display coins
        DisplayCoins();

        // display game mode
        DisplayGameMode();
	}

    // 게임 모드를 표시한다.
    void DisplayGameMode()
    {
        if (gameModeText)
            gameModeText.text = GameController.Me.gameModeController.GetCurGameModeDisplayName();
    }

    public void OnStartButtonClicked()
    {
        GameController.Me.Play();
    }


    // 최고점수 출력
    void DisplayBestScore()
    {
        if (bestScoreText)
        {
            // 모드별로 다르게 표시한다.
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();
            if (curGameMode == GameModeController.GameMode.eEnergyBarMode)
                bestScoreText.text = "Best " + GameController.Me.Player.GameData.EnergyBarModeBestScore.ToString();
            else if (curGameMode == GameModeController.GameMode.e100MMode)
                bestScoreText.text = "Best " + GameMode_100M.TimeToString(GameController.Me.Player.GameData.HundredMBestTime);
        }
    }


    // 점수 출력
    void DisplayScore()
    {
        if(scoreText)
            scoreText.text = GameController.Me.Player.Score.ToString();
    }

    // coins 출력
    void DisplayCoins()
    {
        if (coinsText)
            coinsText.text = GameController.Me.Player.GameData.Coins.ToString();
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
