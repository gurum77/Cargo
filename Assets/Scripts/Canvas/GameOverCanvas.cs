using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;

public class GameOverCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text coinText;

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
	}

    void OnEnable()
    {
    }
    private void DisplayCoins()
    {
        if (coinText)
            coinText.text = StringMaker.GetCoinString();
    }

    private void DisplayBestScore()
    {
        if (bestScoreText)
        {

            // flag 모드에서는 현재 level을 찍어준다.
            if (GameController.Me.gameModeController.GetCurGameMode() == GameModeController.GameMode.eFlagMode)
            {
                bestScoreText.text = StringMaker.GetFlagModeLevelString();
            }
            
            else
            {
                bestScoreText.text = StringMaker.GetBestScoreString();
            }
            
        }
    }

    private void DisplayScore()
    {
        if (scoreText)
        {
            // flag 모드에서는 이기면 win, 지면 lost를 찍는다.
            if (GameController.Me.gameModeController.GetCurGameMode() == GameModeController.GameMode.eFlagMode)
            {
                GameMode_Flag flagMode = GameController.Me.gameModeController.curGameMode.GetComponent<GameMode_Flag>();
                if (flagMode)
                {
                    scoreText.text = flagMode.IsWin() ? "Win" : "Lost";
                }
            }
            else
            {
                scoreText.text = StringMaker.GetScoreString();
            }
        }
            
    }
}
