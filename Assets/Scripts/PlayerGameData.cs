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

    // 최대 콤보 
    static public string MaxComboKey
    {
        get { return "MaxCombo"; }
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

        // max combo
        PlayerPrefs.SetInt(PlayerGameData.MaxComboKey, MaxCombo);
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

        // max combo
        MaxCombo = PlayerPrefs.GetInt(PlayerGameData.MaxComboKey);
    }

    public int EnergyBarModeBestScore
    { get; set; }

    public int Coins
    { get; set; }

    public Player.Character CharacterType
    { get; set; }

    public int MaxCombo
    { get; set; }
    
}
