using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

/// <summary>
/// player game object 스크립트
/// 사용법 : player game object에 추가한다.
/// </summary>
public class Player : MonoBehaviour {
    
    float movingDist = 1.0f;
    public float speed;
    int playerPosition; // 현재 player의 position
    int score;  // 현재 점수
    public AudioSource audioSourceTick;
    public AudioSource audioSourceCoin;
    Vector3 targetPos   = new Vector3();  // 목표 위치

    // player의 게임 데이타
    PlayerGameData gameData = new PlayerGameData();

    public PlayerGameData GameData
    {
        get { return gameData; }
    }
	// Use this for initialization
	void Start () {
       
	}

    void OnEnable()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 45, 0);
        targetPos = new Vector3(0, 0, 0);
        playerPosition = 0;
        score = 0;
        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
            ani.SetTrigger("Car_Base");
    }

    // coin을 추가한다.
    public void AddCoins(int addCoins)
    {
        gameData.Coins = gameData.Coins + addCoins;
    }

    public int Score()
    {
        return score;
    }

    public void OnLeftKeyClicked()
    {
        if (GameController.Me.ControlType == GameController.Control.eControl_TurnAndMove)
            TurnAndMove();
        else
            MoveLeft();
    }

    public void OnRightKeyClicked()
    {
        if (GameController.Me.ControlType == GameController.Control.eControl_TurnAndMove)
            Move();
        else
            MoveRight();
    }

	// Update is called once per frame
	void Update () {

        // play 중인 경우에만 입력을 받는다.
        if(GameController.Me.GameState == GameController.State.ePlay)
        {
            // turn key가 입력 되었는지?
            if (IsInputTurnKey() || IsInputLeftMoveKey())
            {
                OnLeftKeyClicked();
            }

            // move key가 입력 되었는지?
            if (IsInputMoveKey() || IsInputRightMoveKey())
            {
                OnRightKeyClicked();
            }


            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        }
	}

    // 좌측으로 이동시킨다.
    public void MoveLeft()
    {
        if (IsLeftForward())
        {
            Move();
        }
        else
        {
            TurnAndMove();
        }

    }

    // 우측으로 이동시킨다.
    public void MoveRight()
    {
        if(IsLeftForward())
        {
            TurnAndMove();
        }
        else
        {
            Move();
        }
    }

    // player의 방향을 바꾸고 이동한다.
    public void TurnAndMove()
    {
        Turn();
        Move();
    }

    // 왼쪽을 향하고 있는지?
    bool IsLeftForward()
    {
        return transform.forward.x < 0 ? true : false;
    }

    // player의 방향을 바꾼다.
    void Turn()
    {
        if (!enabled)
            return;

        // 좌측 45방향인지?
        if (IsLeftForward())
        { 
            // 우측 45방향을 바라 보도록 한다.
            transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
        }
        else
        {
            // 좌측 45방향을 바라 보도록 한다.
            transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
        }
    }
    
    // player가 이동할 목표 위치를 계산한다.
    void MoveTargetPos()
    {
        if (GameController.Me.roadController.GetRoadBlockProperty(playerPosition) == null)
            return;

        targetPos = GameController.Me.roadController.GetRoadBlockProperty(playerPosition).Position;

        // 이번에 player가 이동해야 할 위치
        targetPos.z += movingDist;
        if (transform.forward.x < 0)
        {
            targetPos.x -= movingDist;
        }
        else
        {
            targetPos.x += movingDist;
        }

        // player position 증가 시킨다.
        playerPosition++;
    }

    // player를 이동한다.
    // 진행방향으로 movingDist만큼 이동한다.
    // move할때마다 playerPosition이 증가한다.
    public void Move()
    {
        if (!enabled)
            return;


        // player가 이동할 목표 위치이동한다.
        MoveTargetPos();
        
        // play
        if (audioSourceTick)
        {
            audioSourceTick.Play();
        }

        // 이동할때마다 체크한다.
        CheckSuccess();

        // Coin 체크
        CheckCoin();

        // map을 갱신한다.
        ReplaceMapByPlayerPosition();
    }

    // 현재 player의 위치에 맞게 map을 갱신한다.
    void ReplaceMapByPlayerPosition()
    {
        // road and map block을 갱신한다.
        ReplaceRoadAndMapBlockByPlayerPosition();

        // coin을 갱신한다.
        ReplaceCoinbyPlayerPosition();
    }

    // player의 위치에 따라 coin을 갱신한다.
    // coin은 road block과 다르게 지나온 자리의 coin은 player가 먹으면서 삭제하므로 여기서 지나간거는 따로 처리 하지 않아도 된다.
    void ReplaceCoinbyPlayerPosition()
    {
        RoadController rc = RoadController();
        if (rc == null)
            return;

        // 추가로 나타나야 하는 coin
        int idx = GetAddRowIndexByPlayerPosition();
        RoadBlockProperty rb = rc.GetRoadBlockProperty(idx);
        if (rb != null)
        {
            rc.MakeCoin(idx, rb.Position, playerPosition);
        }
    }

    // 현재 player의 위치에 맞게 road 와 map block을 갱신한다.
    void ReplaceRoadAndMapBlockByPlayerPosition()
    {
        RoadController rc   = RoadController();
        if(rc == null)
            return;

        // 사라져야 하는 블럭들
        int idx = GetRemoveRowIndexByPlayerPosition();
        RoadBlockProperty rb = rc.GetRoadBlockProperty(idx);
        if(rb != null)
        {
            rb.DeleteAllObjects();
        }

        // 추가로 나타나야 하는 블럭들
        idx = GetAddRowIndexByPlayerPosition();
        rb = rc.GetRoadBlockProperty(idx);
        if(rb != null)
        {
            rc.MakeRoadBlock(idx, rb.Position, playerPosition);
            rc.MakeMapBlockRoadSideByPlayerPosition(idx, playerPosition);
        }
    }
  
    // 현재 player의 위치에서 새롭게 추가(나타나야)되어야 하는 row index
    int GetAddRowIndexByPlayerPosition()
    {
        RoadController rc = RoadController();
        if (rc == null)
            return -1;

        return playerPosition + rc.frontTileRowsFromPlayer - 1;
    }
    // 현재 player의 위치기준으로 사라져야 하는 row index
    int GetRemoveRowIndexByPlayerPosition()
    {
        RoadController rc = RoadController();
        if (rc == null)
            return -1;
        
        return playerPosition - rc.backTileRowsFromPlayer;
    }

    RoadController RoadController()
    {
        if(GameController.Me == false)
            return null;

        return GameController.Me.RoadController;
    }

    // 이동한 위치가 성공인지 체크한다.
    void CheckSuccess()
    {
        if(!RoadController())
            return;
        if (RoadController().GetRoadBlockProperty(playerPosition) == null)
            return;

        Vector3 roadBlockPos    = RoadController().GetRoadBlockProperty(playerPosition).Position;
        
        if (!roadBlockPos.Equals(targetPos))
        {
            // player destory 애니메이션 발동
            Animator ani = GetComponentInChildren<Animator>();
            if(ani != null)
            {
                ani.ResetTrigger("Car_Base"); 
                ani.SetTrigger("Car_Destory");
            }
            GameController.Me.GameOver();
        }
        // 이동한 위치가 성공이면 베스트 스코어를 올린다.
        else
        {
            score = playerPosition;
            GameData.EnergyBarModeBestScore = Score() > GameData.EnergyBarModeBestScore ? Score() : GameData.EnergyBarModeBestScore;
        }
    }

    // 이동한 위치에 코인이 있는지 체크해서 coin개수를 늘린다.
    void CheckCoin()
    {
        if (!RoadController())
            return;

        if (RoadController().GetRoadBlockProperty(playerPosition) == null)
            return;

        // coins의 개수만큼 추가한다.
        if (RoadController().GetRoadBlockProperty(playerPosition).CoinNums > 0)
        {
            if (audioSourceCoin)
                audioSourceCoin.Play();

            // 동전개수를 증가시킨다.
            AddCoins(RoadController().GetRoadBlockProperty(playerPosition).CoinNums);

            // 해당 동전을 삭제한다.
            RoadController().GetRoadBlockProperty(playerPosition).DeleteCoin();

        }
    }

    // turn key가 입력 되었는지?
    // 왼쪽 방향키
    bool IsInputTurnKey()
    {
        if(Input.GetKeyDown("left"))
            return true;
     
        return false;
    }

    // move key가 입력 되었는지?
    // 오른쪽 방향키
    bool IsInputMoveKey()
    {
        if (Input.GetKeyDown("right"))
            return true;
        
        return false;
    }

    // 왼쪽 이동키가 입력되었는지?
    bool IsInputLeftMoveKey()
    { 
        return IsInputTurnKey();
    }

    // 오른쪽 이동키가 입력되었는지?
    bool IsInputRightMoveKey()
    {
        return IsInputMoveKey();
    }
}
