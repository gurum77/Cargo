using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;

/// <summary>
/// Cargo 에서 사용하는 AI이다.
/// </summary>
public class CargoAI : MonoBehaviour {

    // 목표 이동 인터벌(초단위)
    public float targetMovingInterval;
    float curTargetMovingInterval;  // 현재 목표 이동 인터벌

    // 10칸 마다 목표 이동 인터벌을 범위내에서 변경한다.
    // 인터벌 변경할 범위는 목표 이동 인터벌의 지정된 % 내에서 정해진다.
    public float targetMovingIntervalRangeRate;
    public int bossAttackInterval;  // 보스가 공격하는 인터벌
    
    Player player;

    // 보스인지?
    public bool Boss
    { get; set; }

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        player = GetComponentInChildren<Player>();

        curTargetMovingInterval = targetMovingInterval;
    }
	
    // 범위내에서 현재 목표 이동 인터벌을 결정한다.
    void SetCurTargetMovingIntervalInRange()
    {
        float diff = targetMovingInterval * targetMovingIntervalRangeRate;
        curTargetMovingInterval = Random.Range(targetMovingInterval - diff, targetMovingInterval + diff);
    }
	// Update is called once per frame
	void Update () 
    {
        if(player)
        {
            player.EnableUserInput = false;
            player.EnableReplaceMapByPlayerPosition = false;
            
                     
            // interval이 더 커지면 이동한다.
            if (curTargetMovingInterval <= player.MovingInterval)
            {
                // 왜 ai의 ani가 없어지는지 모르겠음(일단 예외처리)
                if(player.Ani == null)
                    player.InitAnimation();

                // 앞으로 1칸 진행한다.
                player.MoveForwardToValidWay(1);


                // 이동을 하고 나서 5칸마다 인터벌을 조정한다.
                if (player.PlayerPosition % 5 == 0)
                {
                    SetCurTargetMovingIntervalInRange();
                }

                // level 별로 바위 공격을 한다.
                // 20칸 마다 공격하자
                if(bossAttackInterval != 0 && player.PlayerPosition % bossAttackInterval == 0)
                {
                    Attack(player.PlayerPosition - 1, MapBlockProperty.ItemType.eRock);
                }
            }
        }
	}

    // com player가 item으로 공격을 한다.
    // 만약 앞,현재,뒤에 blank가 있다면 공격하지 못한다.
    public void Attack(int position, MapBlockProperty.ItemType itemType)
    {
        // 3번째 칸에 바위를 하나 던진다.
        MapBlockProperty prop = GameController.Instance.mapController.GetMapBlockProperty(position);
        if (prop != null)
        {
            MapBlockProperty propPrev   = GameController.Instance.mapController.GetMapBlockProperty(position - 1);
            MapBlockProperty propNext   = GameController.Instance.mapController.GetMapBlockProperty(position + 1);
            if(propPrev != null && propNext != null)
            {
                if (propPrev.Item == MapBlockProperty.ItemType.eBlank)
                    return;
                if (propNext.Item == MapBlockProperty.ItemType.eBlank)
                    return;
                if (prop.Item == MapBlockProperty.ItemType.eBlank)
                    return;
            }

            // 기존 item game object가 있다면 지운다.
            prop.DeleteItemGameObject();

            // 새로운 item type을 설정한다.
            prop.Item = itemType;

            // item game object를 만든다.
            GameController.Instance.mapController.MakeItem(position, prop.Position, position);
        }
    }
    
}
