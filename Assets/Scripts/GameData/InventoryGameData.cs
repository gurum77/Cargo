using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구매 정보
public class PurchaseInfo
{
    public int Price
    { get; set; }

    public int Diamond
    { get; set; }

    public int AD
    { get; set; }


    public bool Enabled
    { get; set; }

    public string Name
    { get; set; }

    public PurchaseInfo()
    {
        Price = 0;
        Diamond = 0;
        AD = 0;
        Enabled = false;
        Name = "";
    }
}

// 맵 구매 정보
public class MapInfo : PurchaseInfo
{
}

/// <summary>
/// 창고 게임 데이타
/// 캐릭터별 특성
/// </summary>

public class CharacterInfo : PurchaseInfo
{
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
    public MapInfo[] mapInfo = new MapInfo[(int)MapController.Map.eCount];
    #endregion

    
    void InitCharacterInfo()
    {
        for (int ix = 0; ix < (int)Player.Character.eCount; ++ix)
        {
            characterInfo[ix] = new CharacterInfo();
        }


        // 기본 케릭터
        characterInfo[(int)Player.Character.eAmbulance].Enabled = true;
        characterInfo[(int)Player.Character.eAmbulance].Price = 0;
        characterInfo[(int)Player.Character.eAmbulance].Name = LocalizationText.GetText("Ambulance");

        // 가격 결정
        characterInfo[(int)Player.Character.eFiretruck].Price = 500;
        characterInfo[(int)Player.Character.eFiretruck].Name = LocalizationText.GetText("Firetruck");

        characterInfo[(int)Player.Character.ePolice].Price = 1000;
        characterInfo[(int)Player.Character.ePolice].Name = LocalizationText.GetText("Police car");
        characterInfo[(int)Player.Character.ePolice].Speed = 8;

        characterInfo[(int)Player.Character.eSportsCar].Price = 0;
        characterInfo[(int)Player.Character.eSportsCar].Diamond = 320;
        characterInfo[(int)Player.Character.eSportsCar].Name = LocalizationText.GetText("Sports car");
        characterInfo[(int)Player.Character.eSportsCar].Speed = 10;
        characterInfo[(int)Player.Character.eSportsCar].CoinRate = 1.5f;

        characterInfo[(int)Player.Character.eTruck].Price = 1000;
        characterInfo[(int)Player.Character.eTruck].Name = LocalizationText.GetText("Truck");
        characterInfo[(int)Player.Character.eTruck].Speed = 4;
        characterInfo[(int)Player.Character.eTruck].Power = 2;

        characterInfo[(int)Player.Character.eTaxi].Price = 2000;
        characterInfo[(int)Player.Character.eTaxi].Name = LocalizationText.GetText("Taxi");

        characterInfo[(int)Player.Character.eVwVan].Price = 2000;
        characterInfo[(int)Player.Character.eVwVan].Name = LocalizationText.GetText("Van");
        characterInfo[(int)Player.Character.eVwVan].Speed = 6;

        characterInfo[(int)Player.Character.ePoliceHelicopter].Price = 0;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Diamond = 200;
        characterInfo[(int)Player.Character.ePoliceHelicopter].Name = LocalizationText.GetText("Police helicopter");
        characterInfo[(int)Player.Character.ePoliceHelicopter].Speed = 7.5f;
        characterInfo[(int)Player.Character.ePoliceHelicopter].CoinRate = 1.2f;


        characterInfo[(int)Player.Character.eInterceptor].Price = 0;
        characterInfo[(int)Player.Character.eInterceptor].Diamond = 250;
        characterInfo[(int)Player.Character.eInterceptor].Name = LocalizationText.GetText("International police car");
        characterInfo[(int)Player.Character.eInterceptor].Speed = 7.5f;
        characterInfo[(int)Player.Character.eInterceptor].CoinRate = 1.2f;


        characterInfo[(int)Player.Character.eCybog].Price = 0;
        characterInfo[(int)Player.Character.eCybog].Diamond = 300;
        characterInfo[(int)Player.Character.eCybog].Name = LocalizationText.GetText("Cybog");
        characterInfo[(int)Player.Character.eCybog].Power = 3;
        characterInfo[(int)Player.Character.eCybog].DefaultLife = 3;

        characterInfo[(int)Player.Character.eDevil].Price = 0;
        characterInfo[(int)Player.Character.eDevil].Diamond = 400;
        characterInfo[(int)Player.Character.eDevil].Name = LocalizationText.GetText("Devil");
        characterInfo[(int)Player.Character.eDevil].DefaultLife = 3;

        characterInfo[(int)Player.Character.eChicken].Price = 10000;
        characterInfo[(int)Player.Character.eChicken].Diamond = 0;
        characterInfo[(int)Player.Character.eChicken].Name = LocalizationText.GetText("Chicken");

        characterInfo[(int)Player.Character.eCondor].Price = 10000;
        characterInfo[(int)Player.Character.eCondor].Diamond = 0;
        characterInfo[(int)Player.Character.eCondor].Name = LocalizationText.GetText("Condor");

        characterInfo[(int)Player.Character.eDragon].Price = 0;
        characterInfo[(int)Player.Character.eDragon].Diamond = 0;
        characterInfo[(int)Player.Character.eDragon].AD = 5;
        characterInfo[(int)Player.Character.eDragon].Name = LocalizationText.GetText("Dragon");
        characterInfo[(int)Player.Character.eDragon].Power = 4;

        characterInfo[(int)Player.Character.eSnowman].Price = 10000;
        characterInfo[(int)Player.Character.eSnowman].Diamond = 0;
        characterInfo[(int)Player.Character.eSnowman].AD = 0;
        characterInfo[(int)Player.Character.eSnowman].Name = LocalizationText.GetText("Snowman");
        characterInfo[(int)Player.Character.eSnowman].Power = 1;

        characterInfo[(int)Player.Character.eCat].Price = 5000;
        characterInfo[(int)Player.Character.eCat].Diamond = 0;
        characterInfo[(int)Player.Character.eCat].AD = 0;
        characterInfo[(int)Player.Character.eCat].Name = LocalizationText.GetText("Cat");
        characterInfo[(int)Player.Character.eCat].Power = 2;

        characterInfo[(int)Player.Character.eLovelyDuck].Price = 0;
        characterInfo[(int)Player.Character.eLovelyDuck].Diamond = 0;
        characterInfo[(int)Player.Character.eLovelyDuck].AD = 5;
        characterInfo[(int)Player.Character.eLovelyDuck].Name = LocalizationText.GetText("Lovely Duck");
        characterInfo[(int)Player.Character.eLovelyDuck].Speed = 3;
        characterInfo[(int)Player.Character.eLovelyDuck].Power = 1;


        characterInfo[(int)Player.Character.eAngryPenguin].Price = 5000;
        characterInfo[(int)Player.Character.eAngryPenguin].Diamond = 0;
        characterInfo[(int)Player.Character.eAngryPenguin].AD = 0;
        characterInfo[(int)Player.Character.eAngryPenguin].Name = LocalizationText.GetText("Angry Penguin");
        characterInfo[(int)Player.Character.eAngryPenguin].Speed = 5.0f;
        characterInfo[(int)Player.Character.eAngryPenguin].Power = 3;

        characterInfo[(int)Player.Character.eFastSheep].Price = 0;
        characterInfo[(int)Player.Character.eFastSheep].Diamond = 300;
        characterInfo[(int)Player.Character.eFastSheep].AD = 0;
        characterInfo[(int)Player.Character.eFastSheep].Name = LocalizationText.GetText("Fast Sheep");
        characterInfo[(int)Player.Character.eFastSheep].Speed = 10.0f;
        characterInfo[(int)Player.Character.eFastSheep].Power = 1;

        characterInfo[(int)Player.Character.eMoleMonster].Price = 5000;
        characterInfo[(int)Player.Character.eMoleMonster].Diamond = 0;
        characterInfo[(int)Player.Character.eMoleMonster].AD = 0;
        characterInfo[(int)Player.Character.eMoleMonster].Name = LocalizationText.GetText("Mole Monster");
        characterInfo[(int)Player.Character.eMoleMonster].Speed = 5.0f;
        characterInfo[(int)Player.Character.eMoleMonster].Power = 2;

        characterInfo[(int)Player.Character.eBlueOldAirplane].Price = 0;
        characterInfo[(int)Player.Character.eBlueOldAirplane].Diamond = 300;
        characterInfo[(int)Player.Character.eBlueOldAirplane].AD = 0;
        characterInfo[(int)Player.Character.eBlueOldAirplane].Name = LocalizationText.GetText("Blue Old Airplane");
        characterInfo[(int)Player.Character.eBlueOldAirplane].Speed = 15.0f;
        characterInfo[(int)Player.Character.eBlueOldAirplane].Power = 2;

        characterInfo[(int)Player.Character.eRedOldAirplane].Price = 0;
        characterInfo[(int)Player.Character.eRedOldAirplane].Diamond = 300;
        characterInfo[(int)Player.Character.eRedOldAirplane].AD = 0;
        characterInfo[(int)Player.Character.eRedOldAirplane].Name = LocalizationText.GetText("Red Old Airplane");
        characterInfo[(int)Player.Character.eRedOldAirplane].Speed = 7.0f;
        characterInfo[(int)Player.Character.eRedOldAirplane].Power = 5;
    }

    void InitMapInfo()
    {
        for (int ix = 0; ix < (int)MapController.Map.eCount; ++ix)
        {
            mapInfo[ix] = new MapInfo();
        }

        mapInfo[(int)MapController.Map.eVillage].Name = LocalizationText.GetText("Village");
        mapInfo[(int)MapController.Map.eVillage].Enabled = true;
        mapInfo[(int)MapController.Map.eVillage].Price = 0;
        mapInfo[(int)MapController.Map.eVillage].Diamond = 0;
        mapInfo[(int)MapController.Map.eVillage].AD = 0;

        mapInfo[(int)MapController.Map.eBasic].Name  = LocalizationText.GetText("Basic");
        mapInfo[(int)MapController.Map.eBasic].Enabled = false;
        mapInfo[(int)MapController.Map.eBasic].Price = 500;
        mapInfo[(int)MapController.Map.eBasic].Diamond = 0;
        mapInfo[(int)MapController.Map.eBasic].AD = 0;

        mapInfo[(int)MapController.Map.eSea].Name = LocalizationText.GetText("Sea");
        mapInfo[(int)MapController.Map.eSea].Enabled = false;
        mapInfo[(int)MapController.Map.eSea].Price = 1000;
        mapInfo[(int)MapController.Map.eSea].Diamond    = 0;
        mapInfo[(int)MapController.Map.eSea].AD = 0;

        mapInfo[(int)MapController.Map.eDesert].Name = LocalizationText.GetText("Desert");
        mapInfo[(int)MapController.Map.eDesert].Enabled = false;
        mapInfo[(int)MapController.Map.eDesert].Price = 3000;
        mapInfo[(int)MapController.Map.eDesert].Diamond = 0;
        mapInfo[(int)MapController.Map.eDesert].AD = 0;

        mapInfo[(int)MapController.Map.eChristmas].Name = LocalizationText.GetText("X-Mas");
        mapInfo[(int)MapController.Map.eChristmas].Enabled = false;
        mapInfo[(int)MapController.Map.eChristmas].Price = 5000;
        mapInfo[(int)MapController.Map.eChristmas].Diamond = 0;
        mapInfo[(int)MapController.Map.eChristmas].AD = 0;

        
        mapInfo[(int)MapController.Map.eForeset].Name = LocalizationText.GetText("Forest");
        mapInfo[(int)MapController.Map.eForeset].Enabled = false;
        mapInfo[(int)MapController.Map.eForeset].Price = 0;
        mapInfo[(int)MapController.Map.eForeset].Diamond = 300;
        mapInfo[(int)MapController.Map.eForeset].AD = 0;
        

    }
    // 초기화
    // 1개만 빼고 모두 비활성화 한다.
    public InventoryGameData()
    {
        InitCharacterInfo();
        InitMapInfo();
       
    }
    #region 게임 데이타 저장 키 정의
    
    static string EnabledKey(Player.Character character)
    {
        return character.ToString() + ".Enabled";
    }

    static string ADKey(Player.Character character)
    {
        return character.ToString() + ".AD";
    }

    static string EnabledKey(MapController.Map map)
    {
        return map.ToString() + ".Enabled";
    }

    static string ADKey(MapController.Map map)
    {
        return map.ToString() + ".AD";
    }

    #endregion

    #region 게임 데이타를 저장하는 함수
    // 게임 데이타를 저장한다.
    public void Save()
    {
        // character
        {
            int count = (int)Player.Character.eCount;
            Player.Character character;
            for (int ix = 0; ix < count; ++ix)
            {
                character = (Player.Character)ix;


                // enabled
                PlayerPrefs.SetInt(InventoryGameData.EnabledKey(character), characterInfo[ix].Enabled ? 1 : 0);

                // 남은 AD
                PlayerPrefs.SetInt(InventoryGameData.ADKey(character), characterInfo[ix].AD);
            }
        }
        

        // map
        {
            int count = (int)MapController.Map.eCount;
            MapController.Map map;
            for (int ix = 0; ix < count; ++ix)
            {
                map = (MapController.Map)ix;


                // enabled
                PlayerPrefs.SetInt(InventoryGameData.EnabledKey(map), mapInfo[ix].Enabled ? 1 : 0);

                // 남은 AD
                PlayerPrefs.SetInt(InventoryGameData.ADKey(map), mapInfo[ix].AD);
            }
        }
    }
    #endregion


    #region 게임 데이타를 읽어오는 함수
    // 게임 데이타를 읽어온다.
    public void Load()
    {
        // character
        {
            int count = (int)Player.Character.eCount;
            Player.Character character;
            for (int ix = 0; ix < count; ++ix)
            {
                character = (Player.Character)ix;

                // enabled
                characterInfo[ix].Enabled = PlayerPrefs.GetInt(InventoryGameData.EnabledKey(character), characterInfo[ix].Enabled ? 1 : 0) == 0 ? false : true;

                // 남은 AD
                characterInfo[ix].AD = PlayerPrefs.GetInt(InventoryGameData.ADKey(character), characterInfo[ix].AD);
            }
        }

        // map
        {
            int count = (int)MapController.Map.eCount;
            MapController.Map map;
            for (int ix = 0; ix < count; ++ix)
            {
                map = (MapController.Map)ix;

                // enabled
                mapInfo[ix].Enabled = PlayerPrefs.GetInt(InventoryGameData.EnabledKey(map), mapInfo[ix].Enabled ? 1 : 0) == 0 ? false : true;

                // 남은 AD
                mapInfo[ix].AD = PlayerPrefs.GetInt(InventoryGameData.ADKey(map), mapInfo[ix].AD);
            }
        }
        
    }
    #endregion
}
