using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour {

    public enum State
    {
        eReady,
        ePlay,
        eGameOver
    };

    // 게임 컨트롤 방식
    public enum Control
    {
        eControl_TurnAndMove,   // turn과 move를 분리
        eControl_MoveOnly       // move만
    };

    public GameObject[] enableGameObjectOnStartup;  // 시작할때 enable 해야 하는 object들..
    public int targetFrameRate;
    static public GameController Instance;
    public MapController mapController;
    public GameModeController gameModeController;
   
    public GameObject playCanvasItems;
    public GameObject readyCanvasItems;
    public GameObject gameOverCanvasItems;

    // 게임 셋팅을 저장하는 데이타
    SettingGameData settingGameData = new SettingGameData();
    public SettingGameData SettingGameData
    {
        get { return settingGameData; }
    }

    // 캐릭터 정보 게임 데이타
    InventoryGameData inventoryGameData;
    public InventoryGameData InventoryGameData
    {
        get { return inventoryGameData; }
    }

    public Player player;
    public Player Player
    {
        get { return player; }
    }

    private Control controlType;
    public Control ControlType
    {
        set { controlType = value;  }
        get { return controlType; }
    }

    private State gameState;
    public State GameState
    {
        set { gameState = value;  }
        get { return gameState; }
    }
    public MapController MapController
    {
        get{return mapController;}
    }

    // game data를 불러온다.
    public void LoadGameData()
    {
        player.GameData.Load();
        settingGameData.Load();
    }

    // game data를 저장한다.
    public void SaveGameData()
    {
        // player의 게임 데이타
        player.GameData.Save();
        settingGameData.Save();
    }

    // 재생을 위해 기다리는 중인지?
    public bool IsWaitingToRevive()
    {
        if (!Player.revivedByADCanvas)
            return false;

        return Player.revivedByADCanvas.gameObject.activeSelf ? true : false;
    }

    // game over를 한다.
    public void GameOver()
    {
        // 데이타 저장
        SaveGameData();

        GameState = State.eGameOver;
        
        // canvas 교체
        playCanvasItems.SetActive(false);
        readyCanvasItems.SetActive(false);
        gameOverCanvasItems.SetActive(true);
        
        player.enabled = false;
        gameModeController.enabled = false;
        gameModeController.DisableAllGameMode();
        mapController.enabled = false;
    }

    // play를 시작한다.
    public void Play()
    {
        // 게임 상태를 플레이로
        GameState = State.ePlay;
        
        // canvas 교체
        playCanvasItems.SetActive(true);
        readyCanvasItems.SetActive(false);
        gameOverCanvasItems.SetActive(false);
        
        // player 활성화
        player.enabled = true;
        player.RevivedByAD = false;
        player.DistXFromCenter = 0.0f;
        player.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


        // player 애니메이션을 base로
        player.InitAnimation();
        

        // game mode controller 활성화
        gameModeController.enabled = true;

        // road controller 활성화
        mapController.enabled = true;

        // 게임 모드 시작
        gameModeController.StartGameMode();
    }

    // 게임 준비단계로 간다.
    public void Ready()
    {
        // 게임 데이타를 동기화 한다.
        {
            SyncGameDataToGameObject();
        }
        

        // 게임 상태를 준비상태로 변경한다.
        GameState = State.eReady;

        // canvas 교체
        playCanvasItems.SetActive(false);
        readyCanvasItems.SetActive(true);
        gameOverCanvasItems.SetActive(false);

        // revive canvas 안 보이게 한다.
        if(Player.revivedByADCanvas)
        {
            Player.revivedByADCanvas.gameObject.SetActive(false);
        }
    }


    // 게임 데이타를 게임 object로 만든다.
    void SyncGameDataToGameObject()
    {
        // character를 교체한다.
        Player.MakeCharacterGameObject();

        // map을 교체한다.
        mapController.ChangeMap(Player.GameData.MapType);

        // game mode를 교체한다
        gameModeController.SetCurGameMode(Player.GameData.GameModeType);


        // 게임 세팅 데이타 동기화
        SyncSettingGameDataToGameObject();
    }

    // 게임 세팅 데이타를 게임 object에 적용한다.
    public void SyncSettingGameDataToGameObject()
    {
        // camera sky view
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController)
        {
            cameraController.skyView = settingGameData.CameraSkyView == 1 ? true : false;
        }
    }

    // 시스템 언어설정으로 localization을 설정한다.
    void SetLocalizationBySystemLanguage()
    {
#if UNITY_EDITOR
        LocalizationText.SetLanguage("KO");
#else
        if (Application.systemLanguage == SystemLanguage.Korean)
            LocalizationText.SetLanguage("KO");
        else
            LocalizationText.SetLanguage("EN");
#endif
    }

    void Awake()
    {
        QualitySettings.vSyncCount = 0;

        SetLocalizationBySystemLanguage();
        Instance = this;
        inventoryGameData = new InventoryGameData();
    }

	// Use this for initialization
	void Start () {

        // 광고 초기화
        Advertisement.Initialize(Define.UnityAds.gameID);

        // 시작할때 활성화해야 하는 아이템
        foreach(var obj in enableGameObjectOnStartup)
        {
            obj.SetActive(true);
        }

        // control type 지정
        controlType = Control.eControl_MoveOnly;

        // 시작할때 게임 데이타를 불러온다.
        LoadGameData();

        // 시작하면 게임 준비 상태
        Ready();
	}
	
	// Update is called once per frame
	void Update () {
        if(targetFrameRate != Application.targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
	}

  
}
