using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 모드 : 100M 전력질주
/// 1000M를 가장 빠른 시간에 달려야 한다.
/// </summary>
public class GameMode_100M : MonoBehaviour {


    float time100M; // 100m 달리는데 걸린 시간
    public GameObject hundredMItem;
    public Text hundredMText;
    public int meter;

	// Use this for initialization
	void Start () {
        time100M = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        // 시간 증가
        IncreaseTime100M();

        // 시간 표시
        DisplayTime100M();

        // 게임 종료 체크
        CheckGameOver();

    }

    // 시간을 string으로 바꾼다.
    static public string TimeToString(float time)
    {
        int sec = (int)(time / 1);
        int msec = (int)((time - (float)sec) * 60.0f);
        return sec.ToString() + ":" + msec.ToString("D2");
    }

    // 기록을 표시한다.
    void DisplayTime100M()
    {
        hundredMText.text = TimeToString(time100M);
    }

    void OnEnable()
    {
        // 여기서 보여야 하는 item을 showItemByGameMode로 이동한다
        if (hundredMItem && GameController.Me && GameController.Me.gameModeController && GameController.Me.gameModeController.showItemByGameMode)
            hundredMItem.transform.SetParent(GameController.Me.gameModeController.showItemByGameMode.transform);

        // 시간 초기화
        time100M = 0.0f;


        // 맵을 구성한다. 100개만 한다
        // 맵을 구성한다.
        if (GameController.Me)
        {
            int maxRoadBlockOld = GameController.Me.mapController.maxRoadBlock;
            GameController.Me.mapController.maxRoadBlock = meter+1;
            GameController.Me.mapController.MakeMap();
            GameController.Me.mapController.maxRoadBlock = maxRoadBlockOld;
        }
    }


    // 100M 시간을 증가시킨다.
    void IncreaseTime100M()
    {
        time100M += Time.deltaTime;
    }

    /// <summary>
    /// 게임 종료를 체크한다.
    /// 스코어가 100이 되면 종료이다.
    /// </summary>
    void CheckGameOver()
    {
        if(GameController.Me.Player.Score >= meter)
        {
            // 결과를 저장한다.
            if (GameController.Me.Player.GameData.HundredMBestTime != 0)
                GameController.Me.Player.GameData.HundredMBestTime = Mathf.Min(time100M, GameController.Me.Player.GameData.HundredMBestTime);
            else
                GameController.Me.Player.GameData.HundredMBestTime = time100M;

            GameController.Me.Player.GameData.Save();

            // player를 목표위치로 이동한다.
            GameController.Me.Player.MoveToTarget();
            // 게임을 종료한다
            GameController.Me.GameOver();
        }
    }
}
