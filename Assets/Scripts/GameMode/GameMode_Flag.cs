using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using UnityEngine.UI;
using ProgressBar;

public class GameMode_Flag : MonoBehaviour {

    // 목표 깃발개수(랜덤하게 변경된다. 50% 내에서 랜덤하게 변경된다)
    public int targetFlagCount;

    // level 10단위로 추가 출현하는 장애물
    public GameObject[] obstacleItemByLevel10;  // 


    // 현재 단계에서의 목표 깃발개수를 생성한다.
    int curTargetFlagCount;
    void GenerateCurTargetFlagCount()
    {
        curTargetFlagCount  = (int)Random.Range(targetFlagCount * 0.5f, targetFlagCount * 1.5f);
    }

    // 장애물 설정
    void GenerateObstacle()
    { 
        // 레벨 10단위당 장애물 출현율을 높인다.
    }

    public float playerScale;
    public float distXFromCenter;

    public GameObject flagModeItem;   // flag 모드 아이템
    public Text playerFlagCountText;
    public Text comFlagCountText;
    
    // com 이동 간격 시작값
    public float startComMovingInterval;

    // 레벨별 com 이동 간격 감소비율
    public float decreaseRateComMovingIntervalByLevel;

    // flag progressbar
    public ProgressBarBehaviour playerProgressbar;
    public ProgressBarBehaviour comProgressbar;


    public Player com;
    public Player Com
    {
        get { return com; }
    }

    public GameObject bossLevelAlert;

	// Use this for initialization
	void Start () {
        curTargetFlagCount = targetFlagCount;
	}
	
	// Update is called once per frame
	void Update () {
        if(IsBossLevel())
        {
            // 보스는 15부터 시작
            if(Com.PlayerPosition < 15)
                Com.PlayerPosition = 15;
        }
    }

    // 목표개수와 현재 개수로 프로그래스바에 입력할 퍼센트 값을 계산하여 리턴한다.
    float GetPercent(int target, int cur)
    {
        return (((float)target - (float)cur) / (float)target) * 100.0f;
    }

    private void FixedUpdate()
    {
        if(GameController.Me && playerProgressbar && comProgressbar)
        {
            playerProgressbar.Value = GetPercent(curTargetFlagCount, GameController.Me.player.FlagCount);
            
            comProgressbar.Value = GetPercent(curTargetFlagCount, Com.FlagCount);
        }
       
       


        CheckSuccess();
    }

    // flag image를 만든다.
    void RecountFlagImage(List<Image> imageList, int flagCount)
    {
        // 지워야 하는 개수
        int removeCount = imageList.Count - flagCount;

        for(int ix = 0; ix < removeCount; ++ix)
        {
            Image image = imageList[imageList.Count - 1];
            DestroyObject(image);
            imageList.RemoveAt(imageList.Count - 1);
        }
    }

    // 성공했는지 체크를 한다.
    void CheckSuccess()
    {
        // player가 깃발 개수를 먼저 채우면 성공
        if (GameController.Me.Player.FlagCount >= curTargetFlagCount)
        {
            GameController.Me.Player.GameData.FlagModeLevel++;
            GameController.Me.GameOver();
        }

        else if (com.FlagCount >= curTargetFlagCount)
        {
            GameController.Me.GameOver();
        }
    }

    public int GetLevel()
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
        if (GameController.Me.Player.FlagCount >= curTargetFlagCount && GameController.Me.Player.FlagCount >= com.FlagCount)
            return true;

        return false;
    }

    
    // 모든 장애물을 비활성화 한다.
    void DisableAllObstacleItem()
    {
        GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eExplosion, false);
        GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eBlank, false);
        GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eRock, false);
    }

    // 레벨별 출현 장애물 설정
    void InitEnableObstacleItem()
    {
        // 현재 레벨
        int level = GetLevel();

        // 단위
        int levelUnit = (level / 10);

        for(int i = 0; i < levelUnit; ++i)
        {
            if(obstacleItemByLevel10.Length <= i)
                break;

            int idx = GameController.Me.mapController.GetItemPrefabIndex(obstacleItemByLevel10[i]);
            if(idx < 0)
                continue;

            GameController.Me.mapController.EnableItem((MapBlockProperty.ItemType)idx, true);
        }
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

        // 목표 깃발 개수를 지정
        GenerateCurTargetFlagCount();


        // 프로그래스바 초기화
        InitProgressbar();
      

        // 맵을 구성한다.
        if (GameController.Me)
        {
            DisableAllObstacleItem();
            InitEnableObstacleItem();

            GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eFlag, true);
            GameController.Me.mapController.MakeMap();
            GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eFlag, false);

            DisableAllObstacleItem();
        }

        // ai player를 활성화 한다.
        SetAIPlayer();
        
        // 보스 레벨 설정
        if(IsBossLevel())
        {
            SetBossLevel();
        }
    }

    // 프로그래스바를 초기화한다.
    void InitProgressbar()
    {
        if (playerProgressbar)
        {
            playerProgressbar.TransitoryValue = 0;
            playerProgressbar.SetFillerSize(100);
        }
        if (comProgressbar)
        {
            comProgressbar.TransitoryValue = 0;
            comProgressbar.SetFillerSize(100);
        }
    }

    // AI player를 설정한다.
    void SetAIPlayer()
    {
        if (com == null)
            return;


        CargoAI ai = com.GetComponent<CargoAI>();
        if (ai)
        {
            // com의 능력치 결정
            // 레벨별로 0.5%씩 빨라진다.
            int level = GetLevel();

            // 보스는 레벨을 10올려서 계산한다.
            if (IsBossLevel())
            {
                level += 10;

                ai.Boss = true;
            }
            else
            {
                ai.Boss = false;
            }

            ai.targetMovingInterval = startComMovingInterval - (startComMovingInterval * ((float)level / 100.0f) * 0.5f);
        }

        // com 캐릭터를 랜덤하게 정해준다.
        com.GameData.CharacterType = (Player.Character)Random.Range(0, (int)Player.Character.eCount - 1);
        com.MakeCharacterGameObject();
        com.enabled = true;

        // com 캐릭터의 위치
        com.DistXFromCenter = distXFromCenter;

        // com 캐릭터의 스케일
        com.transform.localScale = new Vector3(playerScale, playerScale, playerScale);

        // player의 위치
        GameController.Me.Player.DistXFromCenter = distXFromCenter * -1;

        // player의 스케일
        GameController.Me.Player.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
    }

  

    // 보스 레벨을 설정한다.
    void SetBossLevel()
    {
        // 3칸에 바위 공격을 한다.
        CargoAI ai = com.GetComponent<CargoAI>();
        if(ai)
            ai.Attack(3, MapBlockProperty.ItemType.eRock);

        // 보스는 좀 크게 한다.
        com.transform.localScale = new Vector3(2, 2, 2);

        // 경고를 띄운다.
        if(bossLevelAlert)
        {
            bossLevelAlert.SetActive(true);
        }
    }

    // 현재 레벨이 보스 레벨인지?
    bool IsBossLevel()
    {
        return IsBossLevelByLevel(GetLevel());
    }

    // 지정된 레벨이 보스레벨인지?
    public bool IsBossLevelByLevel(int level)
    {
        if (level > 0 && level % 10 == 0)
            return true;

        return false;
    }
    

}
