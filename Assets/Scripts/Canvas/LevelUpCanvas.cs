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
    public Text messageText;
    public Text levelUpText;
    public Text getRewardsText;

	// Use this for initialization
	void Start () {
        int upCount = GameController.Instance.Player.Level - GameController.Instance.LastPlayerLevel;
        // 보상 초기화
        GameController.Instance.Player.CollectedCoins = rewardsCoinsByLevel * upCount;
        GameController.Instance.Player.CollectedDiamonds = rewardsDiamondsByLevel * upCount;

        if (levelText)
        {
            levelText.text = StringMaker.GetLevelString();
        }

        if (messageText)
        {
            messageText.text = LocalizationText.GetText("Congratulations!");
        }
        
        if (levelUpText)
        {
            levelUpText.text = LocalizationText.GetText("Level Up");
        }

        if (getRewardsText)
        {
            getRewardsText.text = LocalizationText.GetText("Get Rewards");
        }
	}
	
	// Update is called once per frame
	void Update () {
	  
	}
  

    public void OnGetButtonClicked()
    {
        GameController.Instance.Player.GetRewards();
        GameController.Instance.Ready();

        GameObject.DestroyObject(gameObject);
    }
}
