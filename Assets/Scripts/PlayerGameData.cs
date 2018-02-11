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
    #region 게임 데이타 저장 키
    static public string CoinsKey
    {
        get { return "Coins"; }
    }

    static public string EnergyBarModeBestScoreKey
    {
        get { return "EnergyBarModeBestScoreKey"; }
    }

    static public string CharacterKey
    {
        get { return "CharacterKey"; }
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

    #endregion

    // 게임 데이타를 저장한다.
    public void Save()
    {
        // coins
        PlayerPrefs.SetInt(PlayerGameData.CoinsKey, Coins);

        // energy bar 모드 최고 점수
        PlayerPrefs.SetInt(PlayerGameData.EnergyBarModeBestScoreKey, EnergyBarModeBestScore);

        // character
        PlayerPrefs.SetInt(PlayerGameData.CharacterKey, (int)CharacterType);

        // game mode
        PlayerPrefs.SetInt(PlayerGameData.GameModeKey, (int)GameModeType);

        // max combo
        PlayerPrefs.SetInt(PlayerGameData.MaxComboKey, MaxCombo);

        // 100M 모드 최고 시간
        PlayerPrefs.SetFloat(PlayerGameData.HundredMModeBestTimeKey, HundredMBestTime);
    }

    // 게임 데이타를 읽어온다.
    public void Load()
    {
        // coins
        Coins = PlayerPrefs.GetInt(PlayerGameData.CoinsKey);

        // energy bar 모드 최고 점수
        EnergyBarModeBestScore = PlayerPrefs.GetInt(PlayerGameData.EnergyBarModeBestScoreKey);

        // character
        CharacterType = (Player.Character)PlayerPrefs.GetInt(PlayerGameData.CharacterKey);

        // game mode
        GameModeType    = (GameModeController.GameMode)PlayerPrefs.GetInt(PlayerGameData.GameModeKey);

        // max combo
        MaxCombo = PlayerPrefs.GetInt(PlayerGameData.MaxComboKey);

        // 100M 모드 최고 기록
        HundredMBestTime = PlayerPrefs.GetFloat(PlayerGameData.HundredMModeBestTimeKey);
    }

    public int EnergyBarModeBestScore
    { get; set; }

    public int Coins
    { get; set; }

    public Player.Character CharacterType
    { get; set; }

    public GameModeController.GameMode GameModeType
    { get; set; }

    public int MaxCombo
    { get; set; }

    public float HundredMBestTime
    { get; set; }
    
}
