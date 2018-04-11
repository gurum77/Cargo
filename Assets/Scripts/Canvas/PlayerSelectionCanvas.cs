using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Controller;
using UnityEngine.UI;
using ProgressBar;
using UnityEngine.Advertisements;

public class PlayerSelectionCanvas : MonoBehaviour {

    Player.Character previewCharacterType;
    GameObject previewCharacter;
    public GameObject[] characterPrefabs;   // player의 캐릭터
    public GameObject player;
    public GameObject curtain;  // 커튼
    public Text coinText;
    public Text diamondText;
    public Text selectText;
    public Text nameText;
    InventoryGameData inventoryGameData;
    PlayerGameData playerGameData;
    public Image coinImage;
    public Image diamondImage;
    public Image adImage;
    public ProgressBarBehaviour powerProgressbar;
    public ProgressBarBehaviour speedProgressbar;
    public ProgressBarBehaviour coinProgressbar;

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


        if (powerProgressbar)
            powerProgressbar.Init();
        if (speedProgressbar)
            speedProgressbar.Init();
        if (coinProgressbar)
            coinProgressbar.Init();

        CreatePreviewCharacter();

        
    }

    // 프로그래스바를 표시한다.
    void DisplayProgressbar()
    {
        int index = (int)previewCharacterType;
        if (index < 0)
            return;

        if(powerProgressbar)
        {
            powerProgressbar.Value = ((float)(playerGameData.AddedPower + inventoryGameData.characterInfo[index].Power) / (float)Define.Max.MaxPower) * 100.0f;  
        }

        if (speedProgressbar)
        {
            speedProgressbar.Value = ((float)(playerGameData.AddedSpeed + inventoryGameData.characterInfo[index].Speed) / (float)Define.Max.MaxSpeed) * 100.0f;
        }

        if (coinProgressbar)
        {
            coinProgressbar.Value = ((float)(playerGameData.AddedCoinRate + inventoryGameData.characterInfo[index].CoinRate) / (float)Define.Max.MaxCoinRate) * 100.0f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Define.Scene.Playground);
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

        // 캐릭터 이름
        if (nameText)
        {
            nameText.text = inventoryGameData.characterInfo[index].Name;
        }

        // price text
        if(coinText)
        {
            coinText.text = playerGameData.Coins.ToString();
        }

        if (diamondText)
        {
            diamondText.text = playerGameData.Diamonds.ToString();
        }

     

        // 비활성화 인 경우 글자를 Get로 바꾼다.
        if(selectText)
        {
            if (inventoryGameData.characterInfo[index].Enabled)
            {
                if (coinImage)
                    coinImage.enabled = false;
                if (diamondImage)
                    diamondImage.enabled = false;
                if (adImage)
                    adImage.enabled = false;

                    selectText.text = LocalizationText.GetText("SELECT");
            }
                
            else
            {
                // diamond로 살수 있는 경우
                if (inventoryGameData.characterInfo[index].Diamond > 0)
                {
                    selectText.text = string.Format("{0}", inventoryGameData.characterInfo[index].Diamond.ToString());
                    if (coinImage)
                        coinImage.enabled = false;
                    if (diamondImage)
                        diamondImage.enabled = true;
                    if (adImage)
                        adImage.enabled = false;
                }
                // coin으로 살수 있는 경우
                else if(inventoryGameData.characterInfo[index].Price > 0)
                {
                    selectText.text = string.Format("${0}", inventoryGameData.characterInfo[index].Price.ToString());
                    if (coinImage)
                        coinImage.enabled = true;
                    if (diamondImage)
                        diamondImage.enabled = false;
                    if (adImage)
                        adImage.enabled = false;
                }
                // AD로 살수 있는 경우
                else if(inventoryGameData.characterInfo[index].AD > 0)
                {
                    selectText.text = string.Format(" x {0}", inventoryGameData.characterInfo[index].AD.ToString());
                    if (coinImage)
                        coinImage.enabled = false;
                    if (diamondImage)
                        diamondImage.enabled = false;
                    if (adImage)
                        adImage.enabled = true;
                }

            }
        }

        DisplayProgressbar();

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

    private void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            int index = (int)previewCharacterType;
            if (index > -1 && previewCharacterType < Player.Character.eCount)
            {
                inventoryGameData.characterInfo[index].AD--;
                if (inventoryGameData.characterInfo[index].AD == 0)
                {
                    inventoryGameData.characterInfo[index].Enabled = true;
                }

                CreatePreviewCharacter();
            }
        }
    }

    // 선택된 캐릭터를 산다.
    bool BuyCharacter()
    {
        int index = (int)previewCharacterType;
        if (index > -1 && previewCharacterType < Player.Character.eCount)
        {
            if (inventoryGameData.characterInfo[index].Diamond > 0)
            {
                if (playerGameData.Diamonds >= inventoryGameData.characterInfo[index].Diamond)
                {
                    playerGameData.Diamonds -= inventoryGameData.characterInfo[index].Diamond;
                    inventoryGameData.characterInfo[index].Enabled = true;

                    return true;
                }
            }
            else if(inventoryGameData.characterInfo[index].Price > 0)
            {
                if (playerGameData.Coins >= inventoryGameData.characterInfo[index].Price)
                {
                    playerGameData.Coins -= inventoryGameData.characterInfo[index].Price;
                    inventoryGameData.characterInfo[index].Enabled = true;

                    return true;
                }
            }
            else if (inventoryGameData.characterInfo[index].AD > 0)
            {
                if (Advertisement.IsReady(Define.UnityAds.rewardedVideo))
                {
                    var options = new ShowOptions { resultCallback = HandleShowResult };
                    Advertisement.Show(Define.UnityAds.rewardedVideo, options);

                    return true;
                }
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
        SceneManager.LoadScene(Define.Scene.Playground);
    }

    // grand ma 버튼 클릭
    public void OnGrandMaButtonClicked()
    {
        ChangePlayer(Player.Character.eCybog);
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
        ChangePlayer(Player.Character.eSportsCar);
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
