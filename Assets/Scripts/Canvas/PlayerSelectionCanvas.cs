using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectionCanvas : MonoBehaviour {

    Player.Character previewCharacterType;
    GameObject previewCharacter;
    public GameObject[] characterPrefabs;   // player의 캐릭터
    public GameObject player;

	// Use this for initialization
	void Start () {

        previewCharacterType = (Player.Character)PlayerPrefs.GetInt(PlayerGameData.CharacterKey);
	}

    void OnEnable()
    {
        CreatePreviewCharacter();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // preview character 를 만든다.
    void CreatePreviewCharacter()
    {
        GameObject prefab = new GameObject();
        int index = (int)previewCharacterType;
        if (index > -1 && previewCharacterType < Player.Character.eCount)
            prefab = characterPrefabs[index];
        else
            prefab = null;

        if(prefab == null)
        {
            Debug.Assert(false);
            return;
        }

        if (previewCharacter)
        {
            GameObject.DestroyObject(previewCharacter);
        }

        previewCharacter    = Instantiate(prefab, player.transform);
        previewCharacter.transform.SetParent(player.transform);
    }

    public void OnLeftButtonClicked()
    {
        if ((int)previewCharacterType > 0)
        {
            previewCharacterType--;
            CreatePreviewCharacter();
        }
    }

    public void OnRightButtonClicked()
    {
        if (previewCharacterType < Player.Character.eCount - 1)
        {
            previewCharacterType++;
            CreatePreviewCharacter();
        }
    }

    public void OnSelectButtonClicked()
    {
        ChangePlayer(previewCharacterType);
    }

    // player를 변경한다.
    void ChangePlayer(Player.Character characterType)
    {
        PlayerPrefs.SetInt(PlayerGameData.CharacterKey, (int)characterType);
        SceneManager.LoadScene("Playground");
    }

    // grand ma 버튼 클릭
    public void OnGrandMaButtonClicked()
    {
        ChangePlayer(Player.Character.eGrandMa);
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

    // police helicopter 버튼 클릭
    public void OnPoliceHelicopterButtonClicked()
    {
        ChangePlayer(Player.Character.ePoliceHelicopter);
    }

}
