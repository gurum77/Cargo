﻿using System;
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

    // volume
    static public string VolumeKey
    {
        get { return "VolumeKey"; }
    }
    #endregion

    #region 게임 세팅 데이타를 저장하는 함수
    // 게임 데이타를 저장한다.
    public void Save()
    {
        // Camera sky view
        PlayerPrefs.SetInt(SettingGameData.CameraSkyViewKey, CameraSkyView);

        // volume
        PlayerPrefs.SetFloat(SettingGameData.VolumeKey, Volume);
    }
    #endregion


    #region 게임 세팅 데이타를 읽어오는 함수
    // 게임 데이타를 읽어온다.
    public void Load()
    {
        // Camera sky view
        CameraSkyView = PlayerPrefs.GetInt(SettingGameData.CameraSkyViewKey);

        // volume
    }
    #endregion

    #region 데이타 get;set;
    public int CameraSkyView
    { get; set; }

    public float Volume
    { get; set; }
    #endregion
}
