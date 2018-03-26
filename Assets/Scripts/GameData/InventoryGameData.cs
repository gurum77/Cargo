﻿using System.Collections;
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

    public float Speed
    { get; set; }

    public int DefaultLife
    { get; set; }

    public int Power
    { get; set; }

    // 코인 획득률
    public float CoinRate
    { get; set; }

    public CharacterInfo()
    {
        Price = 0;
        Diamond = 0;
        Enabled = false;
        Name = "";
        Speed = 7.0f;
        DefaultLife = 2;
        Power = 1;
        CoinRate = 1.0f;
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
        characterInfo[(int)Player.Character.ePolice].Speed = 8;

        characterInfo[(int)Player.Character.eSportsCar].Price = 0;
        characterInfo[(int)Player.Character.eSportsCar].Diamond = 320;
        characterInfo[(int)Player.Character.eSportsCar].Name = LocalizationText.GetText("Sports car");
        characterInfo[(int)Player.Character.eSportsCar].Speed = 10;

        characterInfo[(int)Player.Character.eTruck].Price = 50000;
        characterInfo[(int)Player.Character.eTruck].Name = LocalizationText.GetText("Truck");
        characterInfo[(int)Player.Character.eTruck].Speed = 4;
        characterInfo[(int)Player.Character.eTruck].Power = 2;

        characterInfo[(int)Player.Character.eTaxi].Price = 50000;
        characterInfo[(int)Player.Character.eTaxi].Name = LocalizationText.GetText("Taxi");

        characterInfo[(int)Player.Character.eVwVan].Price = 50000;
        characterInfo[(int)Player.Character.eVwVan].Name = LocalizationText.GetText("Van");
        characterInfo[(int)Player.Character.eVwVan].Speed = 6;
                
        characterInfo[(int)Player.Character.ePoliceHelicopter].Price = 0;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Diamond  = 100;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Name = LocalizationText.GetText("Police helicopter");
        characterInfo[(int)Player.Character.ePoliceHelicopter].Speed = 7.5f;
        characterInfo[(int)Player.Character.ePoliceHelicopter].CoinRate = 1.2f;


        characterInfo[(int)Player.Character.eInterceptor].Price = 0;
        characterInfo[(int)Player.Character.eInterceptor].Diamond = 100;
        characterInfo[(int)Player.Character.eInterceptor].Name = LocalizationText.GetText("International police car");
        characterInfo[(int)Player.Character.eInterceptor].Speed = 7.5f;
        characterInfo[(int)Player.Character.eInterceptor].CoinRate = 1.2f;


        characterInfo[(int)Player.Character.eCybog].Price = 0;
        characterInfo[(int)Player.Character.eCybog].Diamond = 100;
        characterInfo[(int)Player.Character.eCybog].Name = LocalizationText.GetText("Cybog");
        characterInfo[(int)Player.Character.eCybog].Power = 3;
        characterInfo[(int)Player.Character.eCybog].DefaultLife = 3;

        characterInfo[(int)Player.Character.eDevil].Price = 0;
        characterInfo[(int)Player.Character.eDevil].Diamond = 200;
        characterInfo[(int)Player.Character.eDevil].Name = LocalizationText.GetText("Devil");
        characterInfo[(int)Player.Character.eDevil].DefaultLife = 3;

        characterInfo[(int)Player.Character.eChicken].Price = 100000;
        characterInfo[(int)Player.Character.eChicken].Diamond = 0;
        characterInfo[(int)Player.Character.eChicken].Name = LocalizationText.GetText("Chicken");

        characterInfo[(int)Player.Character.eCondor].Price = 100000;
        characterInfo[(int)Player.Character.eCondor].Diamond = 0;
        characterInfo[(int)Player.Character.eCondor].Name = LocalizationText.GetText("Condor");

        characterInfo[(int)Player.Character.eDragon].Price = 300000;
        characterInfo[(int)Player.Character.eDragon].Diamond = 0;
        characterInfo[(int)Player.Character.eDragon].Name = LocalizationText.GetText("Dragon");
        characterInfo[(int)Player.Character.eDragon].Power = 4;
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
