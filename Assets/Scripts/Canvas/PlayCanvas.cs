using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;

public class PlayCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text coinText;
    public Text diamondText;
    public Text comboText;

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
        DisplayCoinsAndDiamonds();

        // display combo
        DisplayCombo();
	}

    // 카메라 버튼 클릭
    public void OnCameraButtonClicked()
    {
        GameController.Me.SettingGameData.CameraSkyView = GameController.Me.SettingGameData.CameraSkyView == 1 ? 0 : 1;
        GameController.Me.SyncSettingGameDataToGameObject();
    }

    // 왼쪽 버튼 클릭
    // 턴
    public void OnLeftButtonClicked()
    {
        GameController.Me.Player.OnLeftKeyClicked();
    }

    // 오른쪽 버튼 클릭
    // 이동
    public void OnRightButtonClicked()
    {
        GameController.Me.Player.OnRightKeyClicked();
    }

    // 점프 버튼 클릭
    public void OnJumpButtonClicked()
    {
        GameController.Me.Player.OnJumpKeyClicked();
    }

    // 최고점수 출력
    void DisplayBestScore()
    {
        if(!bestScoreText)
        {
            Debug.Assert(false);
            return;
        }

        // 모드별로 다르게 표시한다.
        GameModeController.GameMode curMode = GameController.Me.gameModeController.GetCurGameMode();
        if (curMode == GameModeController.GameMode.eEnergyBarMode)
            bestScoreText.text = "Best " + GameController.Me.Player.GameData.EnergyBarModeBestScore.ToString();
        else if (curMode == GameModeController.GameMode.e100MMode)
            bestScoreText.text = "Best " + GameMode_100M.TimeToString(GameController.Me.Player.GameData.HundredMBestTime);
    }

    // 점수 출력
    void DisplayScore()
    {
        if(!scoreText)
        {
            Debug.Assert(false);
            return;
        }

        scoreText.text = GameController.Me.Player.Score.ToString();
    }

    // coins 출력
    void DisplayCoinsAndDiamonds()
    {
        if (coinText)
            coinText.text = StringMaker.GetCoinsString();
        if (diamondText)
            diamondText.text = StringMaker.GetDiamondsString();
    }

    // combo 출력
    void DisplayCombo()
    {
        if(!comboText)
        {
            Debug.Assert(false);
            return;
        }

        if(GameController.Me.player.Combo <= 0)
        {
            comboText.text = "";
        }
        else
        {
            comboText.text = "COMBO " + GameController.Me.player.Combo.ToString();
        }
    }
}
