using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 맵 선택 캔버스
/// </summary>
public class MapSelectionCanvas : MonoBehaviour {

    public ImageSelector imageSelector;
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

    
  

    public void OnImageSelectorSelectButtonClicked()
    {
        if(imageSelector)
        {
            ChangeMap((MapController.Map)imageSelector.SelectedImageIndex);
        }
    }
}
