using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RevivedByADCanvas : MonoBehaviour {

    public Text timerText;
    public int startTime;
    int remainTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        remainTime -= Time.deltaTime;

        if(remainTime <= 0)
        {
            GameObject.Destroy(gameObject);
        }
	}


    void OnEnable()
    {
        remainTime = startTime;
    }


    void HandleShowResult(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            GameController.Me.Player.Life = GameController.Me.Player.Life + 1;
            GameController.Me.Player.RevivedByAD = true;
        }
    }

    

    void OnLifeADButtonClicked()
    {
        // 이미 받았으면 실행불가
        if (GameController.Me.Player.RevivedByAD)
            return;

        // 광고 시청
        if(Advertisement.IsReady(Define.UnityAds.rewardedVideo))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(Define.UnityAds.rewardedVideo, options);
        }

        if(Advertisement.IsReady(Define.UnityAds.rewardedVideo))
        {
            Advertisement.Show()
            
        }

        

        
    }
}
