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
    string CoinsKey
    {
        get { return "Coins"; }
    }

    string EnergyBarModeBestScoreKey
    {
        get { return "EnergyBarModeBestScoreKey"; }
    }

    string CharacterKey
    {
        get { return "CharacterKey"; }
    }

    #endregion

    // 게임 데이타를 저장한다.
    public void Save()
    {
        // coins
        PlayerPrefs.SetInt(CoinsKey, Coins);

        // energy bar 모드 최고 점수
        PlayerPrefs.SetInt(EnergyBarModeBestScoreKey, EnergyBarModeBestScore);

        // character
        PlayerPrefs.SetInt(CharacterKey, (int)Character);
    }

    // 게임 데이타를 읽어온다.
    public void Load()
    {
        // coins
        Coins = PlayerPrefs.GetInt(CoinsKey);

        // energy bar 모드 최고 점수
        EnergyBarModeBestScore = PlayerPrefs.GetInt(EnergyBarModeBestScoreKey);

        // character
        Character = (Player.Character)PlayerPrefs.GetInt(CharacterKey);

    }

    public int EnergyBarModeBestScore
    { get; set; }

    public int Coins
    { get; set; }

    public Player.Character Character
    { get; set; }
    
}
