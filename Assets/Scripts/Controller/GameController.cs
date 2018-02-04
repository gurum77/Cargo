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
    

    static public GameController Me;
    public RoadController roadController;
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
    public RoadController RoadController
    {
        get{return roadController;}
    }

    // game data를 불러온다.
    void LoadGameData()
    {
        player.GameData.Load();
    }

    // game data를 저장한다.
    void SaveGameData()
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
        roadController.enabled = false;

        
    }

    // play를 시작한다.
    public void Play()
    {
        
        GameState = State.ePlay;
        
        // canvas 교체
        playCanvasItems.SetActive(true);
        readyCanvasItems.SetActive(false);
        

        player.enabled = true;
        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
            ani.ResetTrigger("Car_Base");
        gameModeController.enabled = true;
        roadController.enabled = true;

        gameModeController.StartGameMode();
    }

    // 게임 준비단계로 간다.
    public void Ready()
    {
        // Player의 character 를 만든다
        Player.ReplaceCharacter();

        GameState = State.eReady;

        // canvas 교체
        playCanvasItems.SetActive(false);
        readyCanvasItems.SetActive(true);
    }

	// Use this for initialization
	void Start () {
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

       
	}

   
}
