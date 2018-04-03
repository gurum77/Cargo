using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 맵 선택 캔버스
/// </summary>
public class MapSelectionCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Define.Scene.Playground);
        }
	}


    // 맵을 변경한다.
    void ChangeMap(MapController.Map mapType)
    {
        PlayerPrefs.SetInt(PlayerGameData.MapKey, (int)mapType);
        SceneManager.LoadScene(Define.Scene.Playground);
    }

    
    // basic button 클릭
    public void OnBasicButtonClicked()
    {
        ChangeMap(MapController.Map.eBasic);
    }

    // sea button 클릭
    public void OnSeaButtonClicked()
    {
        ChangeMap(MapController.Map.eSea);
    }

    // desert button 클릭
    public void OnDesertButtonClicked()
    {
        ChangeMap(MapController.Map.eDesert);
    }

    // christmas button 클릭
    public void OnChristmasButtonClicked()
    {
        ChangeMap(MapController.Map.eChristmas);
    }
}
