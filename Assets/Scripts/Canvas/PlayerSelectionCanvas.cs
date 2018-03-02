using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Controller;
using UnityEngine.UI;

public class PlayerSelectionCanvas : MonoBehaviour {

    Player.Character previewCharacterType;
    GameObject previewCharacter;
    public GameObject[] characterPrefabs;   // player의 캐릭터
    public GameObject player;
    public GameObject curtain;  // 커튼
    public Text coinText;
    public Text selectText;
    InventoryGameData inventoryGameData;
    PlayerGameData playerGameData;

	// Use this for initialization
	void Start () {
	}

    void OnEnable()
    {
        playerGameData = new PlayerGameData();
        playerGameData.Load();

        inventoryGameData = new InventoryGameData();
        inventoryGameData.Load();

        previewCharacterType = (Player.Character)PlayerPrefs.GetInt(PlayerGameData.CharacterKey);
        CreatePreviewCharacter();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Playground");
        }
	}

    // preview character 를 만든다.
    void CreatePreviewCharacter()
    {
        GameObject prefab = null;
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
            //DestroyObject(previewCharacter);
            GameObject.DestroyObject(previewCharacter);
        }

        previewCharacter    = Instantiate(prefab, player.transform);
        previewCharacter.transform.SetParent(player.transform);

        // 캐릭터가 비활성인 경우 커튼을 보여준다.
        if(curtain)
        {
            curtain.SetActive(inventoryGameData.characterInfo[index].Enabled ? false : true);
        }

        // price text
        if(coinText)
        {
            coinText.text = playerGameData.Coins.ToString();
        }

        // 비활성화 인 경우 글자를 Get로 바꾼다.
        if(selectText)
        {
            if (inventoryGameData.characterInfo[index].Enabled)
                selectText.text = "SELECT";
            else
                selectText.text = string.Format("${0}", inventoryGameData.characterInfo[index].Price.ToString());
        }

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
        int index = (int)previewCharacterType;
        // 비활성화 되어 있다면 구매를 한다.
        if (index > -1 && previewCharacterType < Player.Character.eCount && !inventoryGameData.characterInfo[index].Enabled)
        {
            // 산다.
            if(BuyCharacter())
            {
                CreatePreviewCharacter();
            }
        }
        else
        {
            ChangePlayer(previewCharacterType);
        }
    }

    // 선택된 캐릭터를 산다.
    bool BuyCharacter()
    {
        int index = (int)previewCharacterType;
        if (index > -1 && previewCharacterType < Player.Character.eCount)
        {
            if(playerGameData.Coins >= inventoryGameData.characterInfo[index].Price)
            {
                playerGameData.Coins -= inventoryGameData.characterInfo[index].Price;
                inventoryGameData.characterInfo[index].Enabled = true;

               

                return true;
            }
            
        }

        return false;
    }

    // player를 변경한다.
    void ChangePlayer(Player.Character characterType)
    {
        playerGameData.CharacterType = characterType;
        playerGameData.Save();
        
        inventoryGameData.Save();
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
