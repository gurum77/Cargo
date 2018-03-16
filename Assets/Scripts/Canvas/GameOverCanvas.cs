using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;

public class GameOverCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text coinText;
    public Text diamondText;
    public float timeToShowButtons; // 버튼을 보이게 하는 시간
    float timeFromEnable;   // 활성화 된 이후 시간
    public Button[] hideButtons;    // 숨겨야 하는 버튼을

	// Use this for initialization
	void Start () {
        timeFromEnable = 0.0f;
        HideButtons();
	}
	
	// Update is called once per frame
	void Update () {


        // display score
        DisplayScore();

        // display best
        DisplayBestScore();

        // display coins
        DisplayCoinsAndDiamonds();

        // 시간 누적
        // score text가 안보이면 시간은 항상 0
        if(scoreText.IsActive())
        {
            timeFromEnable += Time.deltaTime;
            ShowButtons();
        }
        else
        {
            timeFromEnable = 0.0f;
            HideButtons();
            
        }
        
	}
    
    // button을 보여준다.
    // 지정된 시간 이후부터 보인다.
    void ShowButtons()
    {
        if (timeFromEnable < timeToShowButtons)
            return;

        foreach (var b in hideButtons)
        {
            b.gameObject.SetActive(true);
        }
    }

    // game over canvas는 게임이 종료되고 나서 3초 뒤에 다른 버튼을 누를 수 있게 한다.
    void OnEnable()
    {
    }

    void HideButtons()
    { 
        foreach(var b in hideButtons)
        {
            b.gameObject.SetActive(false);
        }
    }

    private void DisplayCoinsAndDiamonds()
    {
        if (coinText)
            coinText.text = StringMaker.GetCoinsString();
        if (diamondText)
            diamondText.text = StringMaker.GetDiamondsString();
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
