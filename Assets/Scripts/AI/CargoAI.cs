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

    Player player;

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


                // 이동을 하고 나서 10칸마다 인터벌을 조정한다.
                if (player.PlayerPosition % 10 == 0)
                {
                    SetCurTargetMovingIntervalInRange();
                }
            }
        }
	}

    
    
}
