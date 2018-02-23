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

    Player player;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        player = GetComponentInChildren<Player>();
        player.EnableUserInput = false;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(player)
        {
            // interval이 더 커지면 이동한다.
	    	if(targetMovingInterval <= player.MovingInterval)
            {
                // 앱
                MapBlockProperty prop = GameController.Me.mapController.GetMapBlockProperty(player.PlayerPosition);
                if(prop != null)
                {
                    if (prop.Left)
                        player.MoveLeft();
                    else
                        player.MoveRight();
                }
            }
        }
	}

    
    
}
