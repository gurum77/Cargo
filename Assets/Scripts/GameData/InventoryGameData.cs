using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 창고 게임 데이타
/// 플레이어가 보유한 아이템에 대한 데이타
/// </summary>

public class CharacterInfo
{
    public int Price
    { get; set; }

    public bool Enabled
    { get; set; }
}

public class InventoryGameData
{
    
    #region 데이타 
    public CharacterInfo []characterInfo   = new CharacterInfo[(int)Player.Character.eCount];
    #endregion

    // 초기화
    // 1개만 빼고 모두 비활성화 한다.
    public InventoryGameData()
    {
        for (int ix = 0; ix < (int)Player.Character.eCount; ++ix)
        {
            characterInfo[ix] = new CharacterInfo();
        }
        
        characterInfo[(int)Player.Character.eAmbulance].Enabled = true;
        characterInfo[(int)Player.Character.eAmbulance].Price = 0;

        // 이후 캐릭터는 모드 비활성화
        for (int ix = 1; ix < (int)Player.Character.eCount; ++ix)
        {
            characterInfo[ix].Enabled = false;
        }

        // 가격 결정
        characterInfo[(int)Player.Character.eFiretruck].Price = 50000;
        characterInfo[(int)Player.Character.ePolice].Price = 50000;
        characterInfo[(int)Player.Character.eCar].Price = 50000;
        characterInfo[(int)Player.Character.eTruck].Price = 50000;
        characterInfo[(int)Player.Character.eTaxi].Price = 50000;
        characterInfo[(int)Player.Character.eVwVan].Price = 50000;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Price = 150000;
        characterInfo[(int)Player.Character.eGrandMa].Price = 200000;
    }
    #region 게임 데이타 저장 키 정의
    
    static string EnabledKey(Player.Character character)
    {
        return character.ToString() + ".Enabled";
    }

    #endregion

    #region 게임 데이타를 저장하는 함수
    // 게임 데이타를 저장한다.
    public void Save()
    {
        int count = (int)Player.Character.eCount;
        Player.Character character;
        for (int ix = 0; ix < count; ++ix)
        {
            character = (Player.Character)ix;
            

            // enabled
            PlayerPrefs.SetInt(InventoryGameData.EnabledKey(character), characterInfo[ix].Enabled ? 1 : 0);
        }
    }
    #endregion


    #region 게임 데이타를 읽어오는 함수
    // 게임 데이타를 읽어온다.
    public void Load()
    {
        int count = (int)Player.Character.eCount;
        Player.Character character;
        for (int ix = 0; ix < count; ++ix)
        {
            character = (Player.Character)ix;

            // enabled
            characterInfo[ix].Enabled = PlayerPrefs.GetInt(InventoryGameData.EnabledKey(character), characterInfo[ix].Enabled ? 1 : 0) == 0 ? false : true;
        }
    }
    #endregion
}
