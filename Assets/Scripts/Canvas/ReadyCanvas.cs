using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyCanvas : MonoBehaviour {

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

    public void OnStartButtonClicked()
    {
        GameController.Me.Play();
    }


    // 최고점수 출력
    void DisplayBestScore()
    {
        if (!bestScoreText)
        {
            Debug.Assert(false);
            return;
        }

        bestScoreText.text = "Best " + GameController.Me.Player.GameData.EnergyBarModeBestScore.ToString();
    }


    // 점수 출력
    void DisplayScore()
    {
        if (!scoreText)
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

    // player 선택 버튼 클릭
    public void OnPlayerSelectionButtonClicked()
    {
        // game data를 저장한다.
        GameController.Me.SaveGameData();
        
        // player 선택 신으로 변경한다.
        SceneManager.LoadScene("PlayerSelection");
    }
}
