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

    private void DisplayCoins()
    {
        if (coinText)
            coinText.text = StringMaker.GetCoinString();
    }

    private void DisplayBestScore()
    {
        if (bestScoreText)
            bestScoreText.text = StringMaker.GetBestScoreString();
    }

    private void DisplayScore()
    {
        if (scoreText)
            scoreText.text = StringMaker.GetScoreString();
    }
}
