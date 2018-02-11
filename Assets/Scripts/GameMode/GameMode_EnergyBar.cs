using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProgressBar;

/// <summary>
/// 게임모드 : 에너지바 모드
/// 에너지가 떨어지기 전에 달려야 한다.
/// </summary>
public class GameMode_EnergyBar : MonoBehaviour {

    public float maxEnergy; // 최대 에너지
    public float decreaseEnergyPerSec;  // 초당 줄어드는 에너지
    public float increaseEnergyPerStep; // 한 스탭 이동할때마다 늘어나는 에너지
    public ProgressBarBehaviour progressBar;
    public int lastScore;   // 에너지바에 반영된 마지막 점수
    private float remainEnergy;  // 남은 에너지
    public GameObject energyBarModeItem;   // energy bar mode용 canvas item
    
    
	// Use this for initialization
	void Start () {
        remainEnergy = maxEnergy;
    }
	
  
	// Update is called once per frame
	void Update () {
        // 점수가 변경된 경우 변경된 점수만큼 에너지를 늘린다.
        IncreaseEnergy();

        // 에너지를 줄인다.
        DecreaseEnergy();

        // 에너지 프로그레스바를 갱신한다.
        DisplayEnergyProgressBar();

        // 게임종료 체크
        CheckGameOver();
	}

    void CheckGameOver()
    {
        // 에너지가 0이면 게임을 종료시킨다.
        if(remainEnergy <= 0)
        {
            GameController.Me.GameOver();
        }
    }


    // 모드별 게임 시작할때 호출된다.
    void OnEnable()
    {
        // 여기서 보여야 하는 item을 showItemByGameMode로 이동한다
        if (energyBarModeItem && GameController.Me && GameController.Me.gameModeController && GameController.Me.gameModeController.showItemByGameMode)
            energyBarModeItem.transform.SetParent(GameController.Me.gameModeController.showItemByGameMode.transform);

        // 값 초기화
        lastScore = 0;

        // 현재 에너지를 max로 변경한다
        remainEnergy = maxEnergy;

        // 프로그래스바 값을 최대로 한다.
        progressBar.SetFillerSize(remainEnergy);

        // 맵을 구성한다.
        if (GameController.Me)
            GameController.Me.mapController.MakeMap();

    }

    // 에너지를 늘린다.
    // 추가된 점수만큼 늘려준다.
    void IncreaseEnergy()
    {
        // 현재 점수
        int curScore = GameController.Me.Player.Score;
        // 점수차
        int diffScore = curScore - lastScore;
        
        // 에너지 반영
        remainEnergy += (diffScore * increaseEnergyPerStep);
        if (remainEnergy > 100)
            remainEnergy = 100;

        lastScore = curScore;

    }

    // 에너지를 줄인다.
    void DecreaseEnergy()
    {
        remainEnergy -= (Time.deltaTime * decreaseEnergyPerSec);
        if (remainEnergy < 0)
            remainEnergy = 0;
    }

    // 에너지 프로그레스바를 갱신한다.
    void DisplayEnergyProgressBar()
    {
        progressBar.Value = remainEnergy;
    }
}
