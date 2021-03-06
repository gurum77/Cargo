﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelectionCanvas : MonoBehaviour {

    public ImageSelector imageSelector;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        if(imageSelector)
        {
            imageSelector.SelectImage(PlayerPrefs.GetInt(PlayerGameData.GameModeKey));

            // 번역
            for(int ix = 0; ix < imageSelector.titles.Length; ++ix)
            {
                imageSelector.titles[ix] = GameModeController.GetGameModeDisplayName((GameModeController.GameMode)ix);
            }

            if (imageSelector.selectButtonText)
                imageSelector.selectButtonText.text = LocalizationText.GetText("SELECT");


        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Define.Scene.Playground);
        }
	}

    // 게임모드를 변경한다.
    void ChangeGameMode(GameModeController.GameMode gameMode)
    {
        PlayerPrefs.SetInt(PlayerGameData.GameModeKey, (int)gameMode);
        SceneManager.LoadScene(Define.Scene.Playground);
    }

   
    // 이미지 선택기에서 선택 버튼을 클릭
    public void OnImageSelectorSelectButtonClicked()
    {
        if (imageSelector == null)
            return;

        ChangeGameMode((GameModeController.GameMode)imageSelector.SelectedImageIndex);
    }
}
