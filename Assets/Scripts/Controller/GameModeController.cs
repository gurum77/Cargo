using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 모드를 제어한다.
/// </summary>
public class GameModeController : MonoBehaviour {

    // 게임 모드 종류
    public enum GameMode
    {
        eEnergyBarMode,
        e100MMode,
        eFlagMode,
        eMathMode,
        eUnknown,
        eCount
    };
    public GameObject[] gameModes;
    public GameObject curGameMode;
    public GameObject hiddenItemByGameMode; // 게임 모드별 숨겨지는 canvas item
    public GameObject showItemByGameMode; // 게임 모드별 보여지는 canvas item
    public GameObject[] itemsByGameMode;    // 게임 모드별 모든 아이템

    // 현재 게임 모드를 설정한다.
    public void SetCurGameMode(GameMode gameMode)
    {
        curGameMode = gameModes[(int)gameMode];
    }

    // 현재 게임 모드의 표시 이름리턴
    public string GetCurGameModeDisplayName()
    {
        GameMode gameMode = GetCurGameMode();

        if (gameMode == GameMode.eEnergyBarMode)
            return LocalizationText.GetText("Energy bar");
        else if (gameMode == GameMode.e100MMode)
            return LocalizationText.GetText("100M");
        else if (gameMode == GameMode.eFlagMode)
            return LocalizationText.GetText("Flag");
        else if (gameMode == GameMode.eMathMode)
            return LocalizationText.GetText("Math");

        return "None";
    }
    // 현재 게임 모드를 리턴한다.
    public GameMode GetCurGameMode()
    {
        for(int i = 0; i < (int)GameMode.eCount; ++i)
        {
            if (gameModes[i].ToString() == curGameMode.ToString())
                return (GameMode)i;
        }

        return GameMode.eUnknown;
    }
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
        // 시작할때 모드별 아이템을 숨김아이템 아래로 둔다
        foreach(var item in itemsByGameMode)
        {
            item.transform.SetParent(hiddenItemByGameMode.transform);
        }

        itemsByGameMode[(int)GetCurGameMode()].transform.SetParent(showItemByGameMode.transform);
        

        // item 활성화를 기본값을 설정한다.
        GameController.Me.mapController.SetDefaultEnableItem();

        // hidden item은 숨기고
        hiddenItemByGameMode.SetActive(false);

        // show item은 보인다
        showItemByGameMode.SetActive(true);

        // player의 입력을 활성화 한다.
        // 필요하다면 게임별 모드에서 비활성화 한다.
        GameController.Me.Player.EnableUserInput = true;

        // 게임 모드를 시작한다.
        curGameMode.SetActive(false);
        curGameMode.SetActive(true);
    }
}
