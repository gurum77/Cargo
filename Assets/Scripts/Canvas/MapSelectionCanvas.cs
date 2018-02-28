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
            SceneManager.LoadScene("Playground");
        }
	}


    // 맵을 변경한다.
    void ChangeMap(MapController.Map mapType)
    {
        PlayerPrefs.SetInt(PlayerGameData.MapKey, (int)mapType);
        SceneManager.LoadScene("Playground");
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
}
