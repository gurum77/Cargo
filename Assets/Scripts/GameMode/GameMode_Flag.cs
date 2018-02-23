using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

public class GameMode_Flag : MonoBehaviour {

    // 목표 깃발개수
    public int targetFlagCount;

    public GameObject flagModeItem;   // flag 모드 아이템

    Player aiPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 모드별 게임 시작할때 호출된다.
    void OnEnable()
    {
        // 여기서 보여야 하는 item을 showItemByGameMode로 이동한다
        if (flagModeItem && GameController.Me && GameController.Me.gameModeController && GameController.Me.gameModeController.showItemByGameMode)
            flagModeItem.transform.SetParent(GameController.Me.gameModeController.showItemByGameMode.transform);

        // 값 초기화
        

        // 맵을 구성한다.
        if (GameController.Me)
        {
            GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eFlag, true);
            GameController.Me.mapController.MakeMap();
            GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eFlag, false);
        }
    }
}
