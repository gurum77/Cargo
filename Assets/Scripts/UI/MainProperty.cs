using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;
using ProgressBar;

// player의 주요 속성 객체의 정보를 표시하는 스크립트
// 코인, 레벨, 경험치, 다이아몬드
public class MainProperty : MonoBehaviour {

    public Text coinText;
    public Text diamondText;
    public Text levelText;
    public Text expText;
    public ProgressBarBehaviour progressBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        // display coins
        DisplayCoinsAndDiamonds();

        // display level / exp
        DisplayLevelAndExp();
	}

    // level / exp 출력
    void DisplayLevelAndExp()
    {
        if (levelText)
        {
            levelText.text = StringMaker.GetLevelString();
        }

        if (expText)
        {
            expText.text = StringMaker.GetExpString();
        }

        if(progressBar)
        {
            progressBar.Value = GetPercent();
        }
    }

    float GetPercent()
    {
        int tot = GameController.Instance.Player.expByLevel;
        int cur = GameController.Instance.Player.Exp - GameController.Instance.Player.ExpForCurLevel;
        return ((float)cur / (float)tot) * 100.0f;
    }

    // coins 출력
    void DisplayCoinsAndDiamonds()
    {
        if (coinText)
            coinText.text = StringMaker.GetCoinsString();
        if (diamondText)
            diamondText.text = StringMaker.GetDiamondsString();
    }
}
