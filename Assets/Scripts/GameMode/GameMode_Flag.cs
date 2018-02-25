using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using UnityEngine.UI;

public class GameMode_Flag : MonoBehaviour {

    // 목표 깃발개수
    public int targetFlagCount;

    public GameObject flagModeItem;   // flag 모드 아이템
    public Text playerFlagCountText;
    public Text comFlagCountText;

    public Image playerFlagImage;
    public Image comFlagImage;

    // com 이동 간격 시작값
    public float startComMovingInterval;

    // 레벨별 com 이동 간격 감소비율
    public float decreaseRateComMovingIntervalByLevel;

    public Player com;
    public Player Com
    {
        get { return com; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
        
	}

    private void FixedUpdate()
    {
        if (playerFlagCountText)
        {
            playerFlagCountText.text = StringMaker.GetPlayerFlagCountString();
        }

        if (comFlagCountText)
        {
            comFlagCountText.text = StringMaker.GetComFlagCountString();
        }


        CheckSuccess();
    }

    // 성공했는지 체크를 한다.
    void CheckSuccess()
    {
        // player가 깃발 개수를 먼저 채우면 성공
        if(GameController.Me.Player.FlagCount >= targetFlagCount)
        {
            GameController.Me.Player.GameData.FlagModeLevel++;
            GameController.Me.GameOver();
        }

        else if(com.FlagCount >= targetFlagCount)
        {
            GameController.Me.GameOver();
        }


    }

    int GetLevel()
    {
        if (GameController.Me)
        {
            return GameController.Me.Player.GameData.FlagModeLevel;
        }

        return 1;
    }

    // 이긴 상태인지?
    public bool IsWin()
    {
        // player가 깃발 개수를 먼저 채우면 성공
        if (GameController.Me.Player.FlagCount >= targetFlagCount && GameController.Me.Player.FlagCount >= com.FlagCount)
            return true;

        return false;
    }
    // 모드별 게임 시작할때 호출된다.
    void OnEnable()
    {
        if(GameController.Me == null)
        {
            Debug.Assert(false);
            return;
        }

        // 여기서 보여야 하는 item을 showItemByGameMode로 이동한다
        if (flagModeItem && GameController.Me && GameController.Me.gameModeController && GameController.Me.gameModeController.showItemByGameMode)
            flagModeItem.transform.SetParent(GameController.Me.gameModeController.showItemByGameMode.transform);

      

        // 맵을 구성한다.
        if (GameController.Me)
        {
            GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eFlag, true);
            GameController.Me.mapController.MakeMap();
            GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eFlag, false);
        }

        // ai player를 활성화 한다.
        if(com)
        {
            CargoAI ai = com.GetComponent<CargoAI>();
            if(ai)
            {
                // com의 이동 간격
                ai.targetMovingInterval = startComMovingInterval;

                int level = GetLevel();
                for (int ix = 0; ix < level; ++ix)
                {
                    ai.targetMovingInterval = ai.targetMovingInterval - (ai.targetMovingInterval * decreaseRateComMovingIntervalByLevel);
                }
            }

            // com 캐릭터를 랜덤하게 정해준다.
            com.GameData.CharacterType = (Player.Character)Random.Range(0, (int)Player.Character.eCount - 1);
            com.MakeCharacterGameObject();
            com.enabled = true;
            
        }
            
    }
}
