using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RevivedByADCanvas : MonoBehaviour {

    public Text timerText;
    public int startTime;
    float remainTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        remainTime -= Time.deltaTime;

        if(remainTime <= 0)
        {
            CloseCanvas();
        }

        if(timerText)
        {
            int remainTimeInt = Mathf.RoundToInt(remainTime);
            timerText.text = remainTimeInt.ToString();
        }
	}


    void OnEnable()
    {
        remainTime = startTime;
    }


    // canvas를 닫는다.
    // 게임 object를 삭제하고 pause를 푼다
    void CloseCanvas()
    {
        gameObject.SetActive(false);

        if (GameController.Instance.Player.Life <= 0)
        {
            GameController.Instance.GameOver();
        }
    }

    void HandleShowResult(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            GameController.Instance.Player.Life = GameController.Instance.Player.Life + 1;
            GameController.Instance.Player.RevivedByAD = true;
            CloseCanvas();
        }
    }

    

    public void OnLifeADButtonClicked()
    {
        // 이미 받았으면 실행불가
        if (GameController.Instance.Player.RevivedByAD)
        {
            CloseCanvas();
            return;
        }

        // 광고 시청
        if(Advertisement.IsReady(Define.UnityAds.rewardedVideo))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(Define.UnityAds.rewardedVideo, options);
        }
    }

    // 닫기 버튼 누른 경우
    public void OnCloseButtonClicked()
    {
        CloseCanvas();
    }
}
