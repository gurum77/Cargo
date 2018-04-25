using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Assets.Scripts.Controller;

// 보상 받기 스크립트
public class Rewards : MonoBehaviour {

    public Text collectedCoinsText;
    public Text collectedDiamondsText;
    public Button adButton;
    public Text getButtonText;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        // 보상 표시
        DisplayRewards();

        // 버튼 번역
        if(getButtonText)
        {
            getButtonText.text = LocalizationText.GetText("Get Rewards");
        }
	}

    // 보상을 표시한다.
    void DisplayRewards()
    {
        if (collectedCoinsText)
        {
            collectedCoinsText.text = StringMaker.GetCollectedCoinsString();
        }

        if (collectedDiamondsText)
        {
            collectedDiamondsText.text = StringMaker.GetCollectedDiamondsString();
        }
    }

    // 받기 버튼을 누르면 보상을 받는다.
    // 보상을 받고 나서 ready canvas를 연다.
    public void OnGetButtonClicked()
    {
        GameController.Instance.Player.GetRewards();
        GameController.Instance.Ready();

    }

    private void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            GameController.Instance.Player.DuplicateRewards();
            if (adButton)
            {
                adButton.gameObject.SetActive(false);
            }
        }


    }

    // 광고 버튼 클릭
    public void OnADButtonClicked()
    {
        if (Advertisement.IsReady(Define.UnityAds.rewardedVideo))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(Define.UnityAds.rewardedVideo, options);
        }

    }
}
