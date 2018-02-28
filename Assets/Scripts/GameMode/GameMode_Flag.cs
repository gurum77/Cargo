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
		
	}
	
	// Update is called once per frame
	void Update () {
	
        
	}

    private void FixedUpdate()
    {
        if (playerFlagCountText)
        {
            playerFlagCountText.text = "P";// StringMaker.GetPlayerFlagCountString();

            if (playerFlagImagePrefab)
            {
                RecountFlagImage(playerFlagImageList, targetFlagCount - GameController.Me.Player.FlagCount);
            }
        }

        if (comFlagCountText)
        {
            comFlagCountText.text = "C";// StringMaker.GetComFlagCountString();

            if (comFlagImagePrefab)
            {
                RecountFlagImage(comFlagImageList, targetFlagCount - Com.FlagCount);
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

        
        // 이미지 초기화
        InitImageList();
      

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

    void InitImageList()
    {
        RecountFlagImage(playerFlagImageList, 0);
        RecountFlagImage(comFlagImageList, 0);
        
        // 필요한만큼 만들어둔다.
        if(playerFlagCountText && comFlagCountText && playerFlagImagePrefab && comFlagImagePrefab)
        {
            Vector3 playerFlagImagePos = playerFlagCountText.transform.position;
            for (int ix = 0; ix < targetFlagCount; ++ix)
            {
                Image image = Instantiate(playerFlagImagePrefab, playerFlagCountText.transform);
                playerFlagImagePos.x += flagImageWidth;
                image.transform.position = playerFlagImagePos;
                playerFlagImageList.Add(image);

                if(ix == (targetFlagCount/2)-1)
                {
                    playerFlagImagePos = playerFlagCountText.transform.position;
                    playerFlagImagePos.y -= flagImageHeight;
                }
            }

            Vector3 comFlagImagePos = comFlagCountText.transform.position;
            for (int ix = 0; ix < targetFlagCount; ++ix)
            {
                Image image = Instantiate(comFlagImagePrefab, comFlagCountText.transform);
                comFlagImagePos.x += flagImageWidth;
                image.transform.position = comFlagImagePos;
                comFlagImageList.Add(image);

                if (ix == (targetFlagCount / 2)-1)
                {
                    comFlagImagePos = comFlagCountText.transform.position;
                    comFlagImagePos.y -= flagImageHeight;
                }
            }
        
        }
        
    }
}
