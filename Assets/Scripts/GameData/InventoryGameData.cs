using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 창고 게임 데이타
/// 캐릭터별 특성
/// </summary>

public class CharacterInfo
{
    public int Price
    { get; set; }

    public int Diamond
    { get; set; }

    public bool Enabled
    { get; set; }

    public string Name
    { get; set; }

    public CharacterInfo()
    {
        Price = 0;
        Diamond = 0;
        Enabled = false;
        Name = "";
    }
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

        
        // 기본 케릭터
        characterInfo[(int)Player.Character.eAmbulance].Enabled = true;
        characterInfo[(int)Player.Character.eAmbulance].Price = 0;
        characterInfo[(int)Player.Character.eAmbulance].Name    = LocalizationText.GetText("Ambulance");
        
        // 가격 결정
        characterInfo[(int)Player.Character.eFiretruck].Price = 50000;
        characterInfo[(int)Player.Character.eFiretruck].Name = LocalizationText.GetText("Firetruck");

        characterInfo[(int)Player.Character.ePolice].Price = 50000;
        characterInfo[(int)Player.Character.ePolice].Name = LocalizationText.GetText("Police car");

        characterInfo[(int)Player.Character.eCar].Price = 50000;
        characterInfo[(int)Player.Character.eCar].Name = LocalizationText.GetText("Just car");

        characterInfo[(int)Player.Character.eTruck].Price = 50000;
        characterInfo[(int)Player.Character.eTruck].Name = LocalizationText.GetText("Truck");

        characterInfo[(int)Player.Character.eTaxi].Price = 50000;
        characterInfo[(int)Player.Character.eTaxi].Name = LocalizationText.GetText("Taxi");

        characterInfo[(int)Player.Character.eVwVan].Price = 50000;
        characterInfo[(int)Player.Character.eVwVan].Name = LocalizationText.GetText("Van");
                
        characterInfo[(int)Player.Character.ePoliceHelicopter].Price = 0;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Diamond  = 100;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Name = LocalizationText.GetText("Police helicopter");


        characterInfo[(int)Player.Character.eInterceptor].Price = 0;
        characterInfo[(int)Player.Character.eInterceptor].Diamond = 100;
        characterInfo[(int)Player.Character.eInterceptor].Name = LocalizationText.GetText("International police car");


        characterInfo[(int)Player.Character.eCybog].Price = 0;
        characterInfo[(int)Player.Character.eCybog].Diamond = 100;
        characterInfo[(int)Player.Character.eCybog].Name = LocalizationText.GetText("Cybog");

        characterInfo[(int)Player.Character.eDevil].Price = 0;
        characterInfo[(int)Player.Character.eDevil].Diamond = 200;
        characterInfo[(int)Player.Character.eDevil].Name = LocalizationText.GetText("Devil");

        characterInfo[(int)Player.Character.eChicken].Price = 100000;
        characterInfo[(int)Player.Character.eChicken].Diamond = 0;
        characterInfo[(int)Player.Character.eChicken].Name = LocalizationText.GetText("Chicken");

        characterInfo[(int)Player.Character.eCondor].Price = 100000;
        characterInfo[(int)Player.Character.eCondor].Diamond = 0;
        characterInfo[(int)Player.Character.eCondor].Name = LocalizationText.GetText("Condor");

        characterInfo[(int)Player.Character.eDragon].Price = 300000;
        characterInfo[(int)Player.Character.eDragon].Diamond = 0;
        characterInfo[(int)Player.Character.eDragon].Name = LocalizationText.GetText("Dragon");
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
