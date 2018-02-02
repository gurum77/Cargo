using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        bestScoreText.text = "Best " + GameController.Me.player.GameData.EnergyBarModeBestScore.ToString();
    }


    // 점수 출력
    void DisplayScore()
    {
        if (!scoreText)
        {
            Debug.Assert(false);
            return;
        }

        scoreText.text = GameController.Me.player.Score().ToString();
    }

    // coins 출력
    void DisplayCoins()
    {
        if (!coinsText)
        {
            Debug.Assert(false);
            return;
        }

        coinsText.text = GameController.Me.player.GameData.Coins.ToString();
    }
}
