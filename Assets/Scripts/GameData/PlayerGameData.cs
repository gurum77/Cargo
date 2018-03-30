using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// player별 게임 데이타
/// </summary>
public class PlayerGameData
{
    #region 데이타 get;set;
    public int EnergyBarModeBestScore
    { get; set; }

    public int Coins
    { get; set; }

    public int Diamonds
    { get; set; }

    public Player.Character CharacterType
    { get; set; }

    public MapController.Map MapType
    { get; set; }

    public GameModeController.GameMode GameModeType
    { get; set; }

    public int MaxCombo
    { get; set; }

    public float HundredMBestTime
    { get; set; }

    public int FlagModeLevel
    { get; set; }

    public int AddedPower
    { get; set; }

    public float AddedSpeed
    { get; set; }

    public int AddedDefaultLife
    { get; set; }

    public float AddedCoinRate
    { get; set; }

    public int MathModeBestScore
    { get; set; }

    
    #endregion

    #region 게임 데이타 저장 키 정의
    static public string CoinsKey
    {
        get { return "Coins"; }
    }

    static public string DiamondsKey
    {
        get { return "Diamonds"; }
    }

    static public string EnergyBarModeBestScoreKey
    {
        get { return "EnergyBarModeBestScoreKey"; }
    }

    static public string CharacterKey
    {
        get { return "CharacterKey"; }
    }

    static public string MapKey
    {
        get { return "MapKey"; }
    }

    static public string GameModeKey
    {
        get { return "GameModeKey"; }
    }

    // 최대 콤보 
    static public string MaxComboKey
    {
        get { return "MaxCombo"; }
    }

    // 100M 모드 최고 기록
    static public string HundredMModeBestTimeKey
    {
        get { return "HundredMModeBestTimeKey"; }
    }

    // flag 모드 레벨
    static public string FlagModeLevelKey
    {
        get { return "FlagModeLevelKey"; }
    }

    static public string AddedPowerKey
    {
        get{ return "AddedPowerKey"; }
    }

    static public string AddedSpeedKey
    {
        get{ return "AddedSpeedKey"; }
    }

    static public string AddedDefaultLifeKey
    {
        get{ return "AddedDefaultLifeKey"; }
    }

    static public string AddedCoinRateKey
    {
        get{ return "AddedCoinRateKey"; }
    }

    static public string MathModeBestScoreKey
    {
        get { return "MathModeBestScore"; }
    }

   
    #endregion

    #region 게임 데이타를 저장하는 함수
    // 게임 데이타를 저장한다.
    public void Save()
    {
        // coins
        PlayerPrefs.SetInt(PlayerGameData.CoinsKey, Coins);

        // diamonds
        PlayerPrefs.SetInt(PlayerGameData.DiamondsKey, Diamonds);

        // energy bar 모드 최고 점수
        PlayerPrefs.SetInt(PlayerGameData.EnergyBarModeBestScoreKey, EnergyBarModeBestScore);

        // character
        PlayerPrefs.SetInt(PlayerGameData.CharacterKey, (int)CharacterType);

        // map
        PlayerPrefs.SetInt(PlayerGameData.MapKey, (int)MapType);

        // game mode
        PlayerPrefs.SetInt(PlayerGameData.GameModeKey, (int)GameModeType);

        // max combo
        PlayerPrefs.SetInt(PlayerGameData.MaxComboKey, MaxCombo);

        // 100M 모드 최고 시간
        PlayerPrefs.SetFloat(PlayerGameData.HundredMModeBestTimeKey, HundredMBestTime);

        // flag 모드 레벨
        PlayerPrefs.SetInt(PlayerGameData.FlagModeLevelKey, FlagModeLevel);

        // 추가된 power
        PlayerPrefs.SetInt(PlayerGameData.AddedPowerKey, AddedPower);

        // 추가된 speed
        PlayerPrefs.SetFloat(PlayerGameData.AddedSpeedKey, AddedSpeed);

        // 추가된 default life
        PlayerPrefs.SetInt(PlayerGameData.AddedDefaultLifeKey, AddedDefaultLife);

        // 추가된 coin rate
        PlayerPrefs.SetFloat(PlayerGameData.AddedCoinRateKey, AddedCoinRate);

        // Math mode best score
        PlayerPrefs.SetInt(PlayerGameData.MathModeBestScoreKey, MathModeBestScore);
    }
#endregion


    #region 게임 데이타를 읽어오는 함수
    // 게임 데이타를 읽어온다.
    public void Load()
    {
        // coins
        Coins = PlayerPrefs.GetInt(PlayerGameData.CoinsKey);

        // diamonds
        Diamonds    = PlayerPrefs.GetInt(PlayerGameData.DiamondsKey);

        // energy bar 모드 최고 점수
        EnergyBarModeBestScore = PlayerPrefs.GetInt(PlayerGameData.EnergyBarModeBestScoreKey);

        // character
        CharacterType = (Player.Character)PlayerPrefs.GetInt(PlayerGameData.CharacterKey);

        // map
        MapType = (MapController.Map)PlayerPrefs.GetInt(PlayerGameData.MapKey);

        // game mode
        GameModeType    = (GameModeController.GameMode)PlayerPrefs.GetInt(PlayerGameData.GameModeKey);

        // max combo
        MaxCombo = PlayerPrefs.GetInt(PlayerGameData.MaxComboKey);

        // 100M 모드 최고 기록
        HundredMBestTime = PlayerPrefs.GetFloat(PlayerGameData.HundredMModeBestTimeKey);

        // flag 모드 레벨
        FlagModeLevel = PlayerPrefs.GetInt(PlayerGameData.FlagModeLevelKey);

        // 추가된 power
        AddedPower = PlayerPrefs.GetInt(PlayerGameData.AddedPowerKey);

        // 추가된 speed
        AddedSpeed  = PlayerPrefs.GetFloat(PlayerGameData.AddedSpeedKey);

        // 추가된 default life
        AddedDefaultLife    = PlayerPrefs.GetInt(PlayerGameData.AddedDefaultLifeKey);

        // 추가된 coin rate
        AddedCoinRate   = PlayerPrefs.GetFloat(PlayerGameData.AddedCoinRateKey);

        // Math mode best score
        MathModeBestScore = PlayerPrefs.GetInt(PlayerGameData.MathModeBestScoreKey);

    }
#endregion

  

}
