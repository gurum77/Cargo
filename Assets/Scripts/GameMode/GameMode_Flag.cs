using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using UnityEngine.UI;

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

    public Image playerFlagImagePrefab;
    public Image comFlagImagePrefab;
    List<Image> playerFlagImageList = new List<Image>();
    List<Image> comFlagImageList    = new List<Image>();
    public float flagImageWidth;
    public float flagImageHeight;

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
        curTargetFlagCount = targetFlagCount;
	}
	
	// Update is called once per frame
	void Update () {
	
        
	}

    private void FixedUpdate()
    {
        if (playerFlagCountText)
        {
            playerFlagCountText.text = (curTargetFlagCount - GameController.Me.Player.FlagCount).ToString();// StringMaker.GetPlayerFlagCountString();

            if (playerFlagImagePrefab)
            {
                RecountFlagImage(playerFlagImageList, curTargetFlagCount - GameController.Me.Player.FlagCount);
            }
        }

        if (comFlagCountText)
        {
            comFlagCountText.text = (curTargetFlagCount - Com.FlagCount).ToString();// StringMaker.GetComFlagCountString();

            if (comFlagImagePrefab)
            {
                RecountFlagImage(comFlagImageList, curTargetFlagCount - Com.FlagCount);
            }
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
        if (GameController.Me.Player.FlagCount >= curTargetFlagCount && GameController.Me.Player.FlagCount >= com.FlagCount)
            return true;

        return false;
    }

    
    // 모든 장애물을 비활성화 한다.
    void DisableAllObstacleItem()
    {
        GameController.Me.mapController.EnableItem(MapBlockProperty.ItemType.eExplosion, false);
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


        // 이미지 초기화
        InitImageList();
      

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
        if(com)
        {
            CargoAI ai = com.GetComponent<CargoAI>();
            if(ai)
            {
                // com의 이동 간격
                ai.targetMovingInterval = startComMovingInterval;

                // level에 따라서 제곱근 만큼씩 줄어든다.
                int level = GetLevel();
                float diff = 0.0f;
                for (int ix = 0; ix < level; ++ix)
                {
                    diff = ai.targetMovingInterval * (decreaseRateComMovingIntervalByLevel / (ix+1));
                    
                    ai.targetMovingInterval = ai.targetMovingInterval - diff;
                }
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

    }

    void InitImageList()
    {
        RecountFlagImage(playerFlagImageList, 0);
        RecountFlagImage(comFlagImageList, 0);
        
        // 필요한만큼 만들어둔다.
        if(playerFlagCountText && comFlagCountText && playerFlagImagePrefab && comFlagImagePrefab)
        {
            Vector3 playerFlagImagePos = playerFlagCountText.transform.position;

            for (int ix = 0; ix < curTargetFlagCount; ++ix)
            {
                Image image = Instantiate(playerFlagImagePrefab, playerFlagCountText.transform);
                playerFlagImagePos.x += flagImageWidth;
                image.transform.position = playerFlagImagePos;
                playerFlagImageList.Add(image);

                if (ix == (curTargetFlagCount / 2) - 1)
                {
                    playerFlagImagePos = playerFlagCountText.transform.position;
                    playerFlagImagePos.y -= flagImageHeight;
                }
            }

            Vector3 comFlagImagePos = comFlagCountText.transform.position;
            
            for (int ix = 0; ix < curTargetFlagCount; ++ix)
            {
                Image image = Instantiate(comFlagImagePrefab, comFlagCountText.transform);
                comFlagImagePos.x += flagImageWidth;
                image.transform.position = comFlagImagePos;
                comFlagImageList.Add(image);

                if (ix == (curTargetFlagCount / 2) - 1)
                {
                    comFlagImagePos = comFlagCountText.transform.position;
                    comFlagImagePos.y -= flagImageHeight;
                }
            }
        
        }
        
    }
}
