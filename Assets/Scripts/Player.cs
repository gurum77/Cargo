using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

/// <summary>
/// player game object 스크립트
/// 사용법 : player game object에 추가한다.
/// </summary>
public class Player : MonoBehaviour {
    
    public enum Character
    {
        eAmbulance,
        eFiretruck,
        ePolice,
        eCar,
        eTruck,
        eTaxi,
        eVwVan,
        ePoliceHelicopter,
        eGrandMa,
        eCount
    };

    float movingDist = 1.0f;
    public float speed; // 진행 속도
    public float rotationSpeed; // 회전 속도
    int playerPosition; // 현재 player의 position
    int score;  // 현재 점수
    int combo;  // 정해진 시간내에 이동한 횟수
    public float comboTime; // 콤보로 인정해주는 시간
    float deltaMoveTime;    // 이전 이동에서 현재 이동까지 흐른 시간

    // 변신에 필요한 콤보
    public int level1Combo; // level1 변신에 필요한 콤보


    
    public GameObject[] characterPrefabs;   // player의 캐릭터
    public AudioSource audioSourceTick;
    public AudioSource audioSourceCoin;
    Vector3 targetPos   = new Vector3();  // 목표 위치
    Quaternion targetDir = new Quaternion();   // 목표 방향
    

    // player의 게임 데이타
    PlayerGameData gameData = new PlayerGameData();

    // game data에 맞게 player의 character game object를 만든다.
    // 기존 캐릭터를 삭제하고, 새로운 캐릭터 object를 만든다.
    public void MakeCharacterGameObject()
    {
        // 기존 캐릭터 object 는 삭제한다.
        GameObject character = GameObject.FindGameObjectWithTag("Character");
        if(character)
        {
            GameObject.DestroyObject(character);
        }

        // character enum에 맞는 character prefab를 가져온다.
        GameObject characterPrefab = characterPrefabs[(int)GameData.CharacterType];

        // 새로운 캐릭터 object를 생성한다.
        if(characterPrefab)
        {
            character = Instantiate(characterPrefab, transform);
            character.tag = "Character";
        }
    }

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
        targetDir = transform.rotation;
        playerPosition = 0;
        score = 0;
        Animator ani = GetComponentInChildren<Animator>();
        if (ani != null)
        {
            ani.SetTrigger("Car_Base");
            ani.ResetTrigger("Car_Level1");
            ani.ResetTrigger("Car_Destory");
        }
    }

    // coin을 추가한다.
    public void AddCoins(int addCoins)
    {
        gameData.Coins = gameData.Coins + addCoins;
    }

    public int Combo
    {
        get { return combo; }
    }

    public int Score
    {
        get { return score; }
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


        // 이동에 걸린 시간을 누적시킨다.
        deltaMoveTime += Time.deltaTime;

        // 누적시간이 콤보기준 시간보다 커지면 콤보를 초기화 한다.
        if (deltaMoveTime > comboTime)
            combo = 0;

        // play 중인 경우에만 입력을 받는다.
        if(GameController.Me.GameState == GameController.State.ePlay)
        {
            // 변신 체크
            CheckLevel();

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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetDir, rotationSpeed * Time.deltaTime);
        }

        
    }

    // player를 목표 지점으로 이동한다.(미션 완성했을대 마무리를위한 함수)
    public void MoveToTarget()
    {
        transform.position = targetPos;
        transform.rotation = targetDir;
    }
    int GetLevel()
    {
        if (level1Combo < Combo)
            return 1;

        return 0;
    }

    // level 체크해서 변신한다.
    void CheckLevel()
    {
         Animator ani = GetComponentInChildren<Animator>();
         if (ani == null)
             return;

        int level = GetLevel();
        if (level == 0)
        {
            ani.ResetTrigger("Car_Level1");
            ani.ResetTrigger("Car_Destory");
            ani.SetTrigger("Car_Base");
        }
        else if(level == 1)
        {
            ani.ResetTrigger("Car_Base");
            ani.ResetTrigger("Car_Destory");
            ani.SetTrigger("Car_Level1");
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
        return targetDir.y < 0 ? true : false;
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
            targetDir   = Quaternion.Euler(new Vector3(0, 45, 0));
        }
        else
        {
            // 좌측 45방향을 바라 보도록 한다.
            targetDir   = Quaternion.Euler(new Vector3(0, -45, 0));
        }
    }
    
    // player가 이동할 목표 위치를 계산한다.
    void MoveTargetPos()
    {
        if (GameController.Me.mapController.GetMapBlockProperty(playerPosition) == null)
            return;

        targetPos = GameController.Me.mapController.GetMapBlockProperty(playerPosition).Position;

        // 이번에 player가 이동해야 할 위치
        targetPos.z += movingDist;
        if (this.IsLeftForward())
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
        if(CheckSuccess())
        {
            // Coin 체크
            CheckCoin();

            // map을 갱신한다.
            ReplaceMapByPlayerPosition();

            // combo를 체크한다.
            CheckCombo();
        }
    }

    

    // combo를 체크한다.
    void CheckCombo()
    {
        if (comboTime >= deltaMoveTime)
        { 
            combo++;
        }
        else
        { 
            combo = 0;
        }

        deltaMoveTime = 0;

        // 최대 콤보 갱신
        GameData.MaxCombo = combo > GameData.MaxCombo ? combo : GameData.MaxCombo;
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
        MapController rc = RoadController();
        if (rc == null)
            return;

        // 추가로 나타나야 하는 coin
        int idx = GetAddRowIndexByPlayerPosition();
        MapBlockProperty rb = rc.GetMapBlockProperty(idx);
        if (rb != null)
        {
            rc.MakeCoin(idx, rb.Position, playerPosition);
        }
    }

    // 현재 player의 위치에 맞게 road 와 map block을 갱신한다.
    void ReplaceRoadAndMapBlockByPlayerPosition()
    {
        MapController rc   = RoadController();
        if(rc == null)
            return;

        // 사라져야 하는 블럭들
        int idx = GetRemoveRowIndexByPlayerPosition();
        MapBlockProperty rb = rc.GetMapBlockProperty(idx);
        if(rb != null)
        {
            rb.DeleteAllObjects();
        }

        // 추가로 나타나야 하는 블럭들
        idx = GetAddRowIndexByPlayerPosition();
        rb = rc.GetMapBlockProperty(idx);
        if(rb != null)
        {
            rc.MakeRoadBlock(idx, rb.Position, playerPosition);
            rc.MakeMapBlockRoadSideByPlayerPosition(idx, playerPosition);
        }
    }
  
    // 현재 player의 위치에서 새롭게 추가(나타나야)되어야 하는 row index
    int GetAddRowIndexByPlayerPosition()
    {
        MapController rc = RoadController();
        if (rc == null)
            return -1;

        return playerPosition + rc.frontTileRowsFromPlayer - 1;
    }
    // 현재 player의 위치기준으로 사라져야 하는 row index
    int GetRemoveRowIndexByPlayerPosition()
    {
        MapController rc = RoadController();
        if (rc == null)
            return -1;
        
        return playerPosition - rc.backTileRowsFromPlayer;
    }

    MapController RoadController()
    {
        if(GameController.Me == false)
            return null;

        return GameController.Me.MapController;
    }

    // 이동한 위치가 성공인지 체크한다.
    bool CheckSuccess()
    {
        if(!RoadController())
            return false;
        if (RoadController().GetMapBlockProperty(playerPosition) == null)
            return false;

        Vector3 roadBlockPos    = RoadController().GetMapBlockProperty(playerPosition).Position;
        
        if (!roadBlockPos.Equals(targetPos))
        {
            // player destory 애니메이션 발동
            Animator ani = GetComponentInChildren<Animator>();
            if(ani != null)
            {
                ani.ResetTrigger("Car_Base");
                ani.ResetTrigger("Car_Level1"); 
                ani.SetTrigger("Car_Destory");
            }
            GameController.Me.GameOver();

            return false;
        }
        // 이동한 위치가 성공이면 베스트 스코어를 올린다.
        else
        {
            score = playerPosition;
            GameData.EnergyBarModeBestScore = Score > GameData.EnergyBarModeBestScore ? Score : GameData.EnergyBarModeBestScore;
        }

        return true;
    }

    // 이동한 위치에 코인이 있는지 체크해서 coin개수를 늘린다.
    void CheckCoin()
    {
        if (!RoadController())
            return;

        if (RoadController().GetMapBlockProperty(playerPosition) == null)
            return;

        // coins의 개수만큼 추가한다.
        if (RoadController().GetMapBlockProperty(playerPosition).CoinNums > 0)
        {
            if (audioSourceCoin)
                audioSourceCoin.Play();

            // 동전개수를 증가시킨다.
            AddCoins(RoadController().GetMapBlockProperty(playerPosition).CoinNums);

            // 해당 동전을 삭제한다.
            RoadController().GetMapBlockProperty(playerPosition).DeleteCoin();

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
