using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectionCanvas : MonoBehaviour {

    public GameObject ambulancePrefab;
    public GameObject firetruckPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 앰뷸런스 버튼 클릭
    public void OnAmbulanceButtonClicked()
    {
        PlayerPrefs.SetString("Character", "Ambulance");
        SceneManager.LoadScene("Playground");
    }

    // 파이어트럭 버튼 클릭
    public void OnFiretruckButtonClicked()
    {
        PlayerPrefs.SetString("Character", "Firetruck");
        SceneManager.LoadScene("Playground");

    }
}
