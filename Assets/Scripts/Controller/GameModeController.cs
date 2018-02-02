using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 모드를 제어한다.
/// </summary>
public class GameModeController : MonoBehaviour {

    public GameObject[] gameModes;
    public GameObject curGameMode;

	// Use this for initialization
	void Start () {
        // 시작하면 게임 모드를 일단 모두 비활성화 한다.    
        DisableAllGameMode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 모든 게임 모드를 비활성화 한다.
    public void DisableAllGameMode()
    {
        foreach (var mode in gameModes)
        {
            mode.SetActive(false);
        }
    }

    // 현재 게임 모드를 시작한다.
    public void StartGameMode()
    {
        curGameMode.SetActive(true);
        
    }
}
