using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text coinsText;

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

    // 최고점수 출력
    void DisplayBestScore()
    {
        if(!bestScoreText)
        {
            Debug.Assert(false);
            return;
        }
        
        bestScoreText.text = "Best " + GameController.Me.Player.GameData.EnergyBarModeBestScore.ToString();
    }

    // 점수 출력
    void DisplayScore()
    {
        if(!scoreText)
        {
            Debug.Assert(false);
            return;
        }

        scoreText.text = GameController.Me.Player.Score().ToString();
    }

    // coins 출력
    void DisplayCoins()
    {
        if (!coinsText)
        {
            Debug.Assert(false);
            return;
        }

        coinsText.text = GameController.Me.Player.GameData.Coins.ToString();
    }
}
