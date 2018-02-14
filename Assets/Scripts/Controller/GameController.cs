using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int targetFrameRate;
    static public GameController Me;
    public MapController mapController;
    public GameModeController gameModeController;
   
    public GameObject playCanvasItems;
    public GameObject readyCanvasItems;

    public GameObject characterPrefab;

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
    }

    // game data를 저장한다.
    public void SaveGameData()
    {
        // player의 게임 데이타
        player.GameData.Save();
    }

    // game over를 한다.
    public void GameOver()
    {
        // 데이타 저장
        SaveGameData();

        GameState = State.eGameOver;
        
        // canvas 교체
        playCanvasItems.SetActive(false);
        readyCanvasItems.SetActive(true);
        
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
        
        // player 활성화
        player.enabled = true;

        // player 애니메이션을 base로
        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
            ani.ResetTrigger("Car_Base");

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
    }

	// Use this for initialization
	void Start () {

        QualitySettings.vSyncCount = 0;

        Me = this;

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
