using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;

// level up 축하 
public class LevelUpCanvas : MonoBehaviour {

    public Text levelText;
    public int rewardsCoinsByLevel;
    public int rewardsDiamondsByLevel;

	// Use this for initialization
	void Start () {
        int upCount = GameController.Instance.Player.Level - GameController.Instance.LastPlayerLevel;
        // 보상 초기화
        GameController.Instance.Player.CollectedCoins = rewardsCoinsByLevel * upCount;
        GameController.Instance.Player.CollectedDiamonds = rewardsDiamondsByLevel * upCount;
	}
	
	// Update is called once per frame
	void Update () {
	    if(levelText)
        {
            levelText.text = StringMaker.GetLevelString();
        }
	}

    public void OnGetButtonClicked()
    {
        GameController.Instance.Player.GetRewards();
        GameController.Instance.Ready();

        GameObject.DestroyObject(gameObject);
    }
}
