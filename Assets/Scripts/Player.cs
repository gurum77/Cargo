using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using Assets.Scripts;
using UnityEngine.Advertisements;
using UnityEngine.UI;

/// <summary>
/// player game object 스크립트
/// 사용법 : player game object에 추가한다.
/// </summary>
public class Player : MonoBehaviour {
    
    public enum Character
    {
        eAmbulance=0,
        eFiretruck,
        ePolice,
        eSportsCar,
        eTruck,
        eTaxi,
        eVwVan,
        ePoliceHelicopter,
        eInterceptor,
        eCybog,
        eDevil,
        eChicken,
        eCondor,
        eDragon,
        eSnowman,
        eCat,
        eLovelyDuck,
        eAngryPenguin,
        eFastSheep,
        eMoleMonster,
        eBlueOldAirplane,
        eRedOldAirplane,
        eCount
    };

    // 중심에서 x 방향으로 이동한 거리(기본값은 0이다)
    public float DistXFromCenter
    {
        get;
        set;
    }

    // 이번 게임에서 얻은 코인과 다이아 몬드 ///
    int collectedCoins;
    public int CollectedCoins
    {
        get { return collectedCoins; }
        set { collectedCoins = value; }
    }
    int collectedDiamonds;
    public int CollectedDiamonds
    {
        get { return collectedDiamonds; }
        set { collectedDiamonds = value; }
    }

    ////////////////////////////////////////////

    float movingDist = 1.0f;
    // 현재 life
    int life;
    public int Life
    { 
        get { return life; }
        set { life = value; }
    }

    // 추가된 속성들 ////////
    public int addedDefaultLife;    // 추가된 기본 life 카운트
    public float addedSpeed; // 추가된 진행 속도
    public int addedPower;   // 파워(공격할때 힘)
    public float addedCoinRate; // 추가된 코인 획득률
    ////////////////////////

    public float rotationSpeed; // 회전 속도
    int playerPosition; // 현재 player의 position
    public int PlayerPosition
    { 
        get { return playerPosition; }
        set { playerPosition = value; }
    }

    // 되살아 난적이 있는지?(광고 시청후 되살아 났었는지?)
    public bool RevivedByAD
    {
        get;
        set;
    }
    PlayerGroggy groggy = new PlayerGroggy();

    int score;  // 현재 점수
    int flagCount;   // 현재 깃발 개수
    int combo;  // 정해진 시간내에 이동한 횟수
    public float comboTime; // 콤보로 인정해주는 시간

    float movingInterval;    // 이전 이동에서 현재 이동까지 흐른 시간
    public float MovingInterval
    {
        get { return movingInterval; }
    }

    public GameObject targetGameObject;
    public ParticleSystem turnEffect;
    public ParticleSystem groggyEffect;

    // 변신에 필요한 콤보
    public int level1Combo; // level1 변신에 필요한 콤보
    public int expByLevel;  // level당 경험치


    public HUDText hudTextPrefabs;
    public Canvas hudTextCanvas;
    public Color coinHudTextColor;
    public Color diamondHudTextColor;
    public Color flagHudTextColor;
    public Color clockHudTextColor;
    public Color lifeHudTextColor;
   

    public Canvas revivedByADCanvas;    // 광고로 되살리기 canvas
    public AudioSource audioSourceTick;
    public AudioSource audioSourceCoin;
    public AudioSource audioSourceDiamond;
    public AudioSource audioSourceClock;
    public AudioSource audioSourceLife;
    public AudioSource audioSourceRock;
    public AudioSource audioSourceJump;
    public AudioSource audioSourceDestroy;
    

    Vector3 targetPos   = new Vector3();  // 목표 위치
    Vector3 targetPosWidthDistXFromCenter = new Vector3();  // 중심에서 x거리가 적용된 목표 위치
    Quaternion targetDir = new Quaternion();   // 목표 방향

    // 실제 스피드(기본 speed에 + 추가 speed)
    public float GetRealSpeed()
    {
        return addedSpeed + GameController.Instance.InventoryGameData.characterInfo[(int)GameData.CharacterType].Speed;
    }

    public int GetRealPower()
    {
        return addedPower + GameController.Instance.InventoryGameData.characterInfo[(int)GameData.CharacterType].Power;
    }

    public int GetRealDefaultLife()
    {
        return addedDefaultLife + GameController.Instance.InventoryGameData.characterInfo[(int)GameData.CharacterType].DefaultLife;
    }

    public float GetRealCoinRate()
    {
        return addedCoinRate + GameController.Instance.InventoryGameData.characterInfo[(int)GameData.CharacterType].CoinRate;
    }

   
    // 그로기 상태인지?
    public bool IsGroggy
    {
        get { return groggy.IsGroggy; }
    }
    Animator ani;
    public Animator Ani
    {
        get { return ani; }
    }

    // player의 게임 데이타
    PlayerGameData gameData = new PlayerGameData();

  
    // 사용자 입력이 활성화 되어 있는지?
    public bool EnableUserInput
    {
        get; set;
    }

    // player의 위치에 따라 맵을 갱신할지?
    public bool EnableReplaceMapByPlayerPosition
    { get; set; }


    // child 중에서 tag를 가진 game object를 찾는다.
    GameObject FindChildGameObjectWithTag(string tag)
    {
        foreach(Transform obj in transform)
        {
            if (obj.CompareTag(tag))
                return obj.gameObject;
        }
        return null;
    }
    // game data에 맞게 player의 character game object를 만든다.
    // 기존 캐릭터를 삭제하고, 새로운 캐릭터 object를 만든다.
    public void MakeCharacterGameObject()
    {
        // 기존 캐릭터 object 는 삭제한다.
        GameObject character = FindChildGameObjectWithTag(Define.Tag.Character);
        if(character)
        {
            GameObject.DestroyObject(character);
        }

        // character enum에 맞는 character prefab를 가져온다.
        GameObject characterPrefab = GameController.Instance.characterPrefabs[(int)GameData.CharacterType];

        // 새로운 캐릭터 object를 생성한다.
        if(characterPrefab)
        {
            character = Instantiate(characterPrefab, transform);
            character.tag = Define.Tag.Character;

            // 새로운 캐릭터로 교체하고 나면 animation을 초기화 한다.
            InitAnimation();
        }
    }

    // animation을 초기상태로 설정한다.
    public void InitAnimation()
    {
        ani = GetComponentInChildren<Animator>();
        SetTrigger(Define.Trigger.Base);
    }

    public PlayerGameData GameData
    {
        get { return gameData; }
    }
	// Use this for initialization
	void Start () {
        EnableUserInput = true;
        EnableReplaceMapByPlayerPosition = true;
        RevivedByAD = false;

    }

    void OnEnable()
    {
        // 그로기 상태를 끝낸다.
        groggy.EndGroggy();

        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 45, 0);
        targetPos = new Vector3(0, 0, 0);
        targetPosWidthDistXFromCenter = new Vector3(0, 0, 0);
        targetDir = transform.rotation;
        playerPosition = 0;
        score = 0;
        flagCount = 0;
        collectedCoins = 0;
        collectedDiamonds = 0;
        life = GetRealDefaultLife();

        InitAnimation();
        
    }

    
    // HUDText를 만든다.
    void CreateGetHUDText(string str, Color color)
    {
        if (!hudTextCanvas)
            return;

        // coin을 수집하면 hud text를 띄운다.
        if (!hudTextPrefabs)
            return;

        GameObject hudTextGameObject = GameObject.Instantiate(hudTextPrefabs.gameObject, hudTextPrefabs.transform);
        if (!hudTextGameObject)
            return;

        hudTextGameObject.transform.parent = hudTextCanvas.transform;
        hudTextGameObject.transform.localScale = new Vector3(1, 1, 1);

        HUDText hudText = hudTextGameObject.GetComponentInChildren<HUDText>();
        if (!hudText)
            return;

        hudText.target = this.gameObject;

        Text text = hudText.GetComponentInChildren<Text>();
        if (!text)
            return;

        text.text = str;
        text.color = color;
        
    }

    // 공격용 HUDText를 만든다.
    void CreateAttackHUDText(GameObject target, string str)
    {
        if (!hudTextCanvas)
            return;

        // coin을 수집하면 hud text를 띄운다.
        if (!hudTextPrefabs)
            return;

        GameObject hudTextGameObject = GameObject.Instantiate(hudTextPrefabs.gameObject, hudTextPrefabs.transform);
        if (!hudTextGameObject)
            return;

        hudTextGameObject.transform.parent = hudTextCanvas.transform;
        hudTextGameObject.transform.localScale = new Vector3(1, 1, 1);

        HUDText hudText = hudTextGameObject.GetComponentInChildren<HUDText>();
        if (!hudText)
            return;

        hudText.target = target;

        Text text = hudText.GetComponentInChildren<Text>();
        if (!text)
            return;

        text.fontSize = 50;
        text.color  =  new Color(255, 0, 0);
        text.transform.localScale = new Vector3(1, 1, 1);

        text.text = str;
    }

    // coin을 수집한다.
    public void CollectCoins(int addCoins)
    {
        int realCoins   = (int)((addCoins * (GetTransformLevel() + 1)) * GetRealCoinRate());
        // 현재 레벨에 따라 곱해준다.
        collectedCoins = collectedCoins + realCoins;

        // hud text 생성
        CreateGetHUDText("+" + realCoins.ToString(), coinHudTextColor);
    }

    // diamond를 수집한다.
    public void CollectDiamonds(int addDimonds)
    {
        int realDiamonds = (addDimonds * (GetTransformLevel() + 1));

        // 현재 레벨에 따라 곱해준다.
        collectedDiamonds = collectedDiamonds + realDiamonds;

        // hud text 생성
        CreateGetHUDText("+" + realDiamonds.ToString(), diamondHudTextColor);
    }

    // 보상(coin, diamond)을 2배로 늘린다.
    public void DuplicateRewards()
    {
        collectedCoins *= 2;
        collectedDiamonds *= 2;
    }

    // 보상을 받아서 ame data에 저장한다.
    public void GetRewards()
    {
        gameData.Coins += collectedCoins;
        gameData.Diamonds += collectedDiamonds;
        gameData.Save();
    }

    public int Combo
    {
        get { return combo; }
    }

    public int Score
    {
        get { return score; }
    }

    public int FlagCount
    {
        get { return flagCount; }
    }

    public void OnLeftKeyClicked()
    {
        // 그로기 상태일때는 버튼 클릭 안됨
        if (IsGroggy)
            return;

        if (GameController.Instance.ControlType == GameController.Control.eControl_TurnAndMove)
            TurnAndMove();
        else
            MoveLeft();
    }

    public void OnRightKeyClicked()
    {
        // 그로기 상태일때는 버튼 클릭 안됨
        if (IsGroggy)
            return;


        if (GameController.Instance.ControlType == GameController.Control.eControl_TurnAndMove)
            Move();
        else
            MoveRight();
    }

    public void OnJumpKeyClicked()
    {
        // 그로기 상태일때는 버튼 클릭 안됨
        if (IsGroggy)
            return;


        if(ani)
            ani.SetTrigger(Define.Trigger.Jump);

        // jump를 하면 2칸을 자동으로 진행한다.
        Move(2);
    }

    // 앞으로 step 수만큼 올바른 길로 진행한다.
    public void MoveForwardToValidWay(int step)
    {
        for (int ix = 0; ix < step; ++ix)
        {
            // 다음 칸이 blank라면 점프를 한다.
            MapBlockProperty prop = GameController.Instance.mapController.GetMapBlockProperty(PlayerPosition+1);
            if (prop != null)
            {
                if(prop.Item == MapBlockProperty.ItemType.eBlank)
                {
                    OnJumpKeyClicked();
                    continue;
                }
                
            }

            prop = GameController.Instance.mapController.GetMapBlockProperty(PlayerPosition);
            if (prop != null)
            {
                if (prop.Left)
                    MoveLeft();
                else
                    MoveRight();
            }
        }
    }

    private void FixedUpdate()
    {
        groggy.AddTimeInGroggy(Time.deltaTime);
    }


    // Update is called once per frame
    void Update () {
        if (GameController.Instance.IsWaitingToRevive())
            return;

        // 이동에 걸린 시간을 누적시킨다.
        movingInterval += Time.deltaTime;

        // 누적시간이 콤보기준 시간보다 커지면 콤보를 초기화 한다.
        if (movingInterval > comboTime)
            combo = 0;

        // play 중인 경우에만 입력을 받는다.
        // groggy가 아닐때만 입력을 받는다.
        if(GameController.Instance.GameState == GameController.State.ePlay && !groggy.IsGroggy)
        {
            // 변신 체크
            CheckLevel();

            // turn key가 입력 되었는지?
            if (EnableUserInput)
            {
                if (IsInputTurnKey() || IsInputLeftMoveKey())
                {
                    OnLeftKeyClicked();
                }

                // move key가 입력 되었는지?
                if (IsInputMoveKey() || IsInputRightMoveKey())
                {
                    OnRightKeyClicked();
                }

                // jump key가 입력 되었는지?
                if(IsInputJumpKey())
                {
                    OnJumpKeyClicked();
                }
            }
            

            // 보간이동
            targetPosWidthDistXFromCenter = targetPos;
            targetPosWidthDistXFromCenter.x += DistXFromCenter;

            transform.position = Vector3.Lerp(transform.position, targetPosWidthDistXFromCenter, GetRealSpeed() * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetDir, rotationSpeed * Time.deltaTime);
        }

        if (targetGameObject)
        {
            Vector3 pos = targetPos;
            pos.y = targetGameObject.transform.position.y;
            targetGameObject.transform.position  = pos;
        }
    }

    // player를 목표 지점으로 이동한다.(미션 완성했을대 마무리를위한 함수)
    public void MoveToTarget()
    {
        transform.position = targetPos;
        transform.rotation = targetDir;
    }
    public int GetTransformLevel()
    {
        if (level1Combo < Combo)
            return 1;

        return 0;
    }

    // level 체크해서 변신한다.
    void CheckLevel()
    {
         if (ani == null)
             return;

        int level = GetTransformLevel();
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

        if(turnEffect && !turnEffect.isPlaying)
        {
                
            turnEffect.playOnAwake = true;
            turnEffect.Play();
            turnEffect.enableEmission = true;
        }
    }
    
    // player가 이동할 목표 위치를 계산한다.
    void MoveTargetPos()
    {
        if (GameController.Instance.mapController.GetMapBlockProperty(playerPosition) == null)
            return;

        targetPos = GameController.Instance.mapController.GetMapBlockProperty(playerPosition).Position;

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

    // 레벨
    public int Level
    {
        get
        {
            if (expByLevel == 0)
            {
                Debug.Assert(false);
                return 1;
            }

            return (Exp / expByLevel) + 1;
        }
    }

    // 현재 레벨에 도달하는 경험치
    public int ExpForCurLevel
    {
        get { return (Level-1) * expByLevel; }
    }

    // 다음 레벨에 도달하는 경험치
    public int ExpForNextLevel
    {
        get { return Level * expByLevel; }
    }
    // 경험치 리턴
    public int Exp
    {
        get { return gameData.Exp; }
    }
    
    // 경험치는 추가한다.
    void AddExp(int addedExp)
    {
        // 사용자 입력이 있는 경우에만 경험치를 추가한다.
        if(EnableUserInput)
            gameData.Exp += addedExp;
    }

    // player를 이동한다.
    // 진행방향으로 movingDist만큼 이동한다.
    // move할때마다 playerPosition이 증가한다.
    // 여러 칸을 진행할때는 마지막 칸에서만 성공을 체크한다.
    public void Move(int step=1)
    {
        if (!enabled)
            return;

        // 경험치는 실해해도 준다.
        {
            AddExp(step);
        }
        
        // step 2이상이면 점프 중인것이다.
        if(step > 1 && audioSourceJump)
        {
            audioSourceJump.Play();
        }
        
        bool isJump = false;
        for(int ix = 0; ix < step; ++ix)
        {
            // jump중인지?
            isJump = ix < step - 1 ? true : false;

            // player가 이동할 목표 위치이동한다.
            MoveTargetPos();

            // play
            if (audioSourceTick)
            {
                audioSourceTick.Play();
            }


            // 이동할때마다 체크한다.
            // 마지막 step에서만 이동을 성공했는지 체크한다.
            if (isJump || CheckSuccess())
            {
                // Coin 체크
                CheckItem();

                // map을 갱신한다.
                if (EnableReplaceMapByPlayerPosition)
                    ReplaceMapByPlayerPosition();

                // combo를 체크한다.
                CheckCombo();
            }
        }

      
    }

    

    // combo를 체크한다.
    void CheckCombo()
    {
        if (comboTime >= movingInterval)
        { 
            combo++;
        }
        else
        { 
            combo = 0;
        }

        movingInterval = 0;

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
            rc.MakeItem(idx, rb.Position, playerPosition);
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
            rb.DeleteAllGameObjects();
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
        
        return playerPosition - rc.backTileRowsFromPlayer - Mathf.RoundToInt(rc.MaxMapSize.z);
    }

    MapController RoadController()
    {
        if(GameController.Instance == false)
            return null;

        return GameController.Instance.MapController;
    }
   
    // 데미지를 준다.
    // return false이면 게임 종료
    public bool ApplyDamage()
    {
        // 성공을 못 하면 카메라를 흔든다.
        Camera.main.SendMessage(Define.Message.Clash);

        // player destory 애니메이션 발동
        SetTrigger(Define.Trigger.Destroy);
       
        // life를 하나 깐다.
        // 단 life가 0개가 되면 죽는다.
        life--;

        if(life == 0)
        {
            if (audioSourceDestroy)
                audioSourceDestroy.Play();
        }
        CreateAttackHUDText(this.gameObject, "-1");

        // 아직 살아있다면 이전위치로 이동한다.
        if (life > 0 || (EnableUserInput && life <= 0 && RevivedByAD == false && revivedByADCanvas))
        {
            // life가 0인데 광고로 살아난 적이 없다면
            // 광고 시청 기회를 준다.
            if (life <= 0)
                revivedByADCanvas.gameObject.SetActive(true);

            MoveToPrevPosition();

            // 1동안 움직이지 못한다.(그로기 상태이다)
            groggy.StartGroggy();

            // 그로기 효과
            if (groggyEffect)
                groggyEffect.Play();

            if (audioSourceRock)
            {
                audioSourceRock.Play();
            }
        }
        else
        {
          

            GameController.Instance.GameOver();

            return false;
        }
        
        return true;
    }

    // 모든 animation을 reset을 하고 지정된 trigger만 set을 한다.
    void SetTrigger(string trigger)
    {
        if (ani == null)
            return;

       
        if(trigger != Define.Trigger.Base)
            ani.ResetTrigger(Define.Trigger.Base);
        if(trigger != Define.Trigger.Level1)
            ani.ResetTrigger(Define.Trigger.Level1);
        if(trigger != Define.Trigger.Destroy)
            ani.ResetTrigger(Define.Trigger.Destroy);

        ani.SetTrigger(trigger);
    }

    // 이동한 위치가 성공인지 체크한다.
    bool CheckSuccess()
    {
        if(!RoadController())
            return false;

        MapBlockProperty prop = RoadController().GetMapBlockProperty(playerPosition);
        if (prop == null)
            return false;

        Vector3 roadBlockPos    = prop.Position;
        // 장애물 블럭이면 실패(점프를 하면 체크하지 않는다)
        // 잘못 이동한 위치라면 실패
        if (prop.Item == MapBlockProperty.ItemType.eBlank || !roadBlockPos.x.Equals(targetPos.x) || !roadBlockPos.z.Equals(targetPos.z))
        {
            if (!ApplyDamage())
                return false;
        }
        // 이동한 위치가 성공이면 베스트 스코어를 올린다.
        else
        {
            score = playerPosition;

            // 성공할때마다 현재 게임모드에 최고기록 갱신을 요청한다.
            GameController.Instance.gameModeController.RefreshBestScoreWithCurrentState();
       }

        return true;
    }

    // 이동한 위치에 item이 있는지 체크해서 item적용
    void CheckItem()
    {
        if (!RoadController())
            return;

        MapBlockProperty prop = RoadController().GetMapBlockProperty(playerPosition);
        if (prop == null)
            return;

        // coin인 경우
        if (prop.Item == MapBlockProperty.ItemType.eCoin || prop.Item == MapBlockProperty.ItemType.eBigCoin)
        {
            if (audioSourceCoin)
                audioSourceCoin.Play();

            // 동전개수를 증가시킨다.
            CollectCoins(prop.GetCoinNums());
        }
        // Diamond인 경우
        else if (prop.Item == MapBlockProperty.ItemType.eDiamond)
        {
            if (audioSourceDiamond)
                audioSourceDiamond.Play();

            // diamond 개수를 증가시킨다.
            CollectDiamonds(prop.GetDiamondNums());
        }
        // flag인 경우
        else if(prop.Item == MapBlockProperty.ItemType.eFlag)
        {
            if (audioSourceCoin)
                audioSourceCoin.Play();

            // flag 개수를 증가시킨다.
            flagCount++;

            CreateGetHUDText("+1", flagHudTextColor);
        }
        // clock인 경우
        else if(prop.Item == MapBlockProperty.ItemType.eClock)
        {
            if (audioSourceClock)
                audioSourceClock.Play();

            // 시간을 늘린다.
            GameMode_EnergyBar energyBarMode = GameController.Instance.gameModeController.curGameMode.GetComponent < GameMode_EnergyBar>();
            if(energyBarMode)
            {
                energyBarMode.IncreateEnergyByAmount(5);

                CreateGetHUDText("+5", clockHudTextColor);
            }
        }
        // life인 경우
        else if(prop.Item == MapBlockProperty.ItemType.eLife)
        {
            if (audioSourceLife)
                audioSourceLife.Play();

            life++;

            CreateGetHUDText("+1", lifeHudTextColor);
        }
        // lock인 경우
        else if(prop.Item == MapBlockProperty.ItemType.eRock)
        {
            if (audioSourceRock)
                audioSourceRock.Play();

            // rock의 health가 남아 있다면 player를 원래 위치로 이동한다.
            if (prop.IsRemainHealth())
            {
                // 공격력을 표시
                CreateAttackHUDText(prop.ItemGameObject, "-" + GetRealPower().ToString());

                // damage를 준다.
                prop.AddDamage(GetRealPower());

                // 원래 위치로 이동
                MoveToPrevPosition();
                return;
            }
        }

        // 해당 item을 삭제한다.
        prop.DeleteItemGameObject();
    }

    // player를 이전 위치로 옮긴다.
    void MoveToPrevPosition()
    {
        if (playerPosition == 0)
            return;

        playerPosition--;
        targetPos = GameController.Instance.mapController.GetMapBlockProperty(playerPosition).Position;
        // 살짝 흔든다.
        
    }

    // turn key가 입력 되었는지?
    // 왼쪽 방향키
    bool IsInputTurnKey()
    {
        if(Input.GetKeyDown(Define.Key.Left))
            return true;
     
        return false;
    }

    // jump 키가 입력 되었는지?
    // 스페이스
    bool IsInputJumpKey()
    {
        if (Input.GetKeyDown(Define.Key.Space))
            return true;

        return false;
    }

    // move key가 입력 되었는지?
    // 오른쪽 방향키
    bool IsInputMoveKey()
    {
        if (Input.GetKeyDown(Define.Key.Right))
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
