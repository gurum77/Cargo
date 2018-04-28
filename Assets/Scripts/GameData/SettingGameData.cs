using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 게임 셋팅 데이타
/// </summary>
public class SettingGameData
{
    #region 게임 세팅 데이타 저장 키 정의
  

    // camera sky view 설정
    static public string CameraSkyViewKey
    {
        get { return "CameraSkyViewKey"; }
    }

    // sound volume
    static public string SoundVolumeKey
    {
        get { return "SoundVolumeKey"; }
    }

    // sound onoff
    static public string SoundOnOffKey
    {
        get { return "SoundOnOffKey"; }
    }

    // music volume
    static public string MusicVolumeKey
    {
        get { return "MusicVolumeKey"; }
    }

    // music onoff
    static public string MusicOnOffKey
    {
        get { return "MusicOnOffKey"; }
    }
    #endregion

    #region 게임 세팅 데이타를 저장하는 함수
    // 게임 데이타를 저장한다.
    public void Save()
    {
        // Camera sky view
        PlayerPrefs.SetInt(SettingGameData.CameraSkyViewKey, CameraSkyView);

        // sound volume
        PlayerPrefs.SetFloat(SettingGameData.SoundVolumeKey, SoundVolume);

        // sound onoff
        PlayerPrefs.SetInt(SettingGameData.SoundOnOffKey, SoundOnOff);

        // music volume
        PlayerPrefs.SetFloat(SettingGameData.MusicVolumeKey, MusicVolume);

        // music onoff
        PlayerPrefs.SetInt(SettingGameData.MusicOnOffKey, MusicOnOff);
    }
    #endregion


    #region 게임 세팅 데이타를 읽어오는 함수
    // 게임 데이타를 읽어온다.
    public void Load()
    {
        // Camera sky view
        CameraSkyView = PlayerPrefs.GetInt(SettingGameData.CameraSkyViewKey, 1);

        // sound volume
        SoundVolume = PlayerPrefs.GetFloat(SettingGameData.SoundVolumeKey, 0.7f);

        // sound on
        SoundOnOff = PlayerPrefs.GetInt(SettingGameData.SoundOnOffKey, 1);

        // music volume
        MusicVolume = PlayerPrefs.GetFloat(SettingGameData.MusicVolumeKey, 0.7f);

        // music on/off
        MusicOnOff  = PlayerPrefs.GetInt(SettingGameData.MusicOnOffKey, 1);
    }
    #endregion

    #region 데이타 get;set;
    public int CameraSkyView
    { get; set; }

    public float SoundVolume
    { get; set; }

    public int SoundOnOff
    { get; set; }

    public float MusicVolume
    { get; set; }

    public int MusicOnOff
    { get; set; }

    #endregion
}
