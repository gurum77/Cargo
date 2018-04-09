using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour {

    public Text scoreText;
    public Text bestScoreText;
    public Text coinText;
    public Text diamondText;
    public Text gameOverText;
    public float timeToShowButtons; // 버튼을 보이게 하는 시간
    float timeFromEnable;   // 활성화 된 이후 시간
    public Button[] hideButtons;    // 숨겨야 하는 버튼을
    public Image successImage;
    public Image failImage;
    public Image bestImage;

    public Text collectedCoinsText;
    public Text collectedDiamondsText;

	// Use this for initialization
	void Start () {
        timeFromEnable = 0.0f;
        if (gameOverText)
            gameOverText.text = LocalizationText.GetText("GameOver");
        HideButtons();
	}
	
	// Update is called once per frame
	void Update () {

        
        // display score
        DisplayScore();

        // display best
        DisplayBestScore();

        // display coins
        DisplayCoinsAndDiamonds();

        // 시간 누적
        // score text가 안보이면 시간은 항상 0
        if(scoreText.IsActive())
        {
            timeFromEnable += Time.deltaTime;
            ShowButtons();
        }
        else
        {
            timeFromEnable = 0.0f;
            HideButtons();
        }
        
        // 결과 이미지 표시
        DisplayResultImage();

        // 보상 표시
        DisplayRewards();
	}

    // 광고 버튼 클릭
    public void OnADButtonClicked()
    {
        GameController.Me.Player.DuplicateRewards();
    }

    // 결과 이미지를 표시한다.
    void DisplayResultImage()
    {
        // 성공인지 실패에 따라 적절한 이미지를 보여준다.
        Image resultImage   = failImage;
        if (IsBest())
            resultImage = bestImage;
        else if (IsSuccess())
            resultImage = successImage;

        if (resultImage && resultImage.transform.localScale.x == 0)
        {
            Animator ani = resultImage.GetComponentInChildren<Animator>();
            if (ani && ani.isActiveAndEnabled)
            {
                ani.SetTrigger(Define.Trigger.Stamp_Take);
            }
        }
    }

    // 보상을 표시한다.
    void DisplayRewards()
    {
        if(collectedCoinsText)
        {
            collectedCoinsText.text = StringMaker.GetCollectedCoinsString();
        }

        if(collectedDiamondsText)
        {
            collectedDiamondsText.text = StringMaker.GetCollectedDiamondsString();
        }
    }
    
    // button을 보여준다.
    // 지정된 시간 이후부터 보인다.
    void ShowButtons()
    {
        if (timeFromEnable < timeToShowButtons)
            return;

        foreach (var b in hideButtons)
        {
            if(b)
                b.gameObject.SetActive(true);
        }
    }

    // game over canvas는 게임이 종료되고 나서 3초 뒤에 다른 버튼을 누를 수 있게 한다.
    void OnEnable()
    {
      

    }

    void HideButtons()
    { 
        foreach(var b in hideButtons)
        {
            b.gameObject.SetActive(false);
        }
    }

    private void DisplayCoinsAndDiamonds()
    {
        if (coinText)
            coinText.text = StringMaker.GetCoinsString();
        if (diamondText)
            diamondText.text = StringMaker.GetDiamondsString();
    }

    private void DisplayBestScore()
    {
        if (bestScoreText)
        {
            GameModeController.GameMode curGameMode = GameController.Me.gameModeController.GetCurGameMode();

            // flag 모드에서는 현재 level을 찍어준다.
            if (curGameMode == GameModeController.GameMode.eFlagMode)
            {
                bestScoreText.text = StringMaker.GetFlagModeLevelString();
            }
            else
            {
                bestScoreText.text = StringMaker.GetBestScoreString();
            }
            
        }
    }

    // best인지?
    // 100M 모드에서 최고 기록 달성했으면 베스트
    // energy 모드에서 최고 기록 달성했으면 베스트
    // flag 모드에서 보스를 깼으면 베스트
    bool IsBest()
    {
        GameModeController modeController   = GameController.Me.gameModeController;
        Player player   = GameController.Me.Player;

        if (modeController.GetCurGameMode() == GameModeController.GameMode.eFlagMode)
        {
            GameMode_Flag flagMode = modeController.curGameMode.GetComponent<GameMode_Flag>();
            if (flagMode && flagMode.IsWin() && flagMode.IsBossLevelByLevel(flagMode.GetLevel() - 1))
                return flagMode.IsWin();
        }
        else if(modeController.GetCurGameMode() == GameModeController.GameMode.eEnergyBarMode)
        {
            if (player.Score == player.GameData.EnergyBarModeBestScore)
                return true;
        }
        else if(modeController.GetCurGameMode() == GameModeController.GameMode.e100MMode)
        {
            GameMode_100M hundredMode = modeController.curGameMode.GetComponent<GameMode_100M>();
            if (hundredMode && hundredMode.Time100M == player.GameData.HundredMBestTime)
                return true;
        }

        return false;
    }

    // 성공인지?
    // flag 모드에서를 제외하고 모든 모드는 캐릭터가 데미지를 입어서 끝났다면 실패다.
    bool IsSuccess()
    {
        // flag 모드에서는 이기면 win, 지면 lost를 찍는다.
        if (GameController.Me.gameModeController.GetCurGameMode() == GameModeController.GameMode.eFlagMode)
        {
            GameMode_Flag flagMode = GameController.Me.gameModeController.curGameMode.GetComponent<GameMode_Flag>();
            if (flagMode)
                return flagMode.IsWin();
        }

        return GameController.Me.Player.Life == 0 ? false : true;
    }

    private void DisplayScore()
    {
        if (scoreText)
        {
            // flag 모드에서는 이기면 win, 지면 lost를 찍는다.
            if (GameController.Me.gameModeController.GetCurGameMode() == GameModeController.GameMode.eFlagMode)
            {
                GameMode_Flag flagMode = GameController.Me.gameModeController.curGameMode.GetComponent<GameMode_Flag>();
                if (flagMode)
                {
                    scoreText.text = flagMode.IsWin() ? "Win" : "Lost";
                }
            }
            else
            {
                scoreText.text = StringMaker.GetScoreString();
            }
        }
            
    }

    // 받기 버튼을 누르면 보상을 받는다.
    // 보상을 받고 나서 ready canvas를 연다.
    public void OnGetButtonClicked()
    {
        GameController.Me.Player.GetRewards();
        GameController.Me.Ready();
        
    }
}
