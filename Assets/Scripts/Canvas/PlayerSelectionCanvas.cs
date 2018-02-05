using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectionCanvas : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // player를 변경한다.
    void ChangePlayer(Player.Character characterType)
    {
        PlayerPrefs.SetInt(PlayerGameData.CharacterKey, (int)characterType);
        SceneManager.LoadScene("Playground");
    }

    // 앰뷸런스 버튼 클릭
    public void OnAmbulanceButtonClicked()
    {
        ChangePlayer(Player.Character.eAmbulance);
    }

    // 파이어트럭 버튼 클릭
    public void OnFiretruckButtonClicked()
    {
        ChangePlayer(Player.Character.eFiretruck);
    }

    // Police 버튼 클릭
    public void OnPoliceButtonClicked()
    {
        ChangePlayer(Player.Character.ePolice);
    }

    // Car 버튼 클릭
    public void OnCarButtonClicked()
    {
        ChangePlayer(Player.Character.eCar);
    }

    // Truck 버튼 클릭
    public void OnTruckButtonClicked()
    {
        ChangePlayer(Player.Character.eTruck);
    }

    // Taxi 버튼 클릭
    public void OnTaxiButtonClicked()
    {
        ChangePlayer(Player.Character.eTaxi);
    }

    // VwVan 버튼 클릭
    public void OnVwVanButtonClicked()
    {
        ChangePlayer(Player.Character.eVwVan);
    }
}
