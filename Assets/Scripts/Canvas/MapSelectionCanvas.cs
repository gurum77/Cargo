using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/// <summary>
/// 맵 선택 캔버스
/// </summary>
public class MapSelectionCanvas : MonoBehaviour {

    public ImageSelector imageSelector;
    
    // purchase
    public Text coinText;
    public Text diamondText;
    public Text selectText;
    public Image coinImage;
    public Image diamondImage;
    public Image adImage;

    InventoryGameData inventoryGameData;
    PlayerGameData playerGameData;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        if(imageSelector)
        {
            imageSelector.SelectImage(PlayerPrefs.GetInt(PlayerGameData.MapKey));

            // 번역
            for(int ix = 0; ix < imageSelector.titles.Length; ++ix)
            {
                imageSelector.titles[ix] = MapController.GetMapName((MapController.Map)ix);
            }

            if (imageSelector.selectButtonText)
                imageSelector.selectButtonText.text = LocalizationText.GetText("SELECT");
        }

        playerGameData = new PlayerGameData();
        playerGameData.Load(); 
        
        inventoryGameData = new InventoryGameData();
        inventoryGameData.Load();

        DisplayPurchaseInfo();
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
        playerGameData.MapType = mapType;
        playerGameData.Save();
        inventoryGameData.Save();

        SceneManager.LoadScene(Define.Scene.Playground);
    }

    public void OnLeftButtonClicked()
    {
        imageSelector.OnLeftButtonClicked();
        DisplayPurchaseInfo();
    }

    public void OnRightButtonClicked()
    {
        imageSelector.OnRightButtonClicked();
        DisplayPurchaseInfo();
    }


    // 구매 정보를 표시한다.
    void DisplayPurchaseInfo()
    {
        int index = (int)imageSelector.SelectedImageIndex;

        // price text
        if (coinText)
        {
            coinText.text = playerGameData.Coins.ToString();
        }

        if (diamondText)
        {
            diamondText.text = playerGameData.Diamonds.ToString();
        }



        // 비활성화 인 경우 글자를 Get로 바꾼다.
        if (selectText)
        {
            if (inventoryGameData.mapInfo[index].Enabled)
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
                if (inventoryGameData.mapInfo[index].Diamond > 0)
                {
                    selectText.text = string.Format("{0}", inventoryGameData.mapInfo[index].Diamond.ToString());
                    if (coinImage)
                        coinImage.enabled = false;
                    if (diamondImage)
                        diamondImage.enabled = true;
                    if (adImage)
                        adImage.enabled = false;
                }
                // coin으로 살수 있는 경우
                else if (inventoryGameData.mapInfo[index].Price > 0)
                {
                    selectText.text = string.Format("{0}", inventoryGameData.mapInfo[index].Price.ToString());
                    if (coinImage)
                        coinImage.enabled = true;
                    if (diamondImage)
                        diamondImage.enabled = false;
                    if (adImage)
                        adImage.enabled = false;
                }
                // AD로 살수 있는 경우
                else if (inventoryGameData.mapInfo[index].AD > 0)
                {
                    selectText.text = string.Format(" x {0}", inventoryGameData.mapInfo[index].AD.ToString());
                    if (coinImage)
                        coinImage.enabled = false;
                    if (diamondImage)
                        diamondImage.enabled = false;
                    if (adImage)
                        adImage.enabled = true;
                }

            }
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            int index = (int)imageSelector.SelectedImageIndex;
            if (index > -1 && index < (int)MapController.Map.eCount)
            {
                inventoryGameData.mapInfo[index].AD--;
                if (inventoryGameData.mapInfo[index].AD == 0)
                {
                    inventoryGameData.mapInfo[index].Enabled = true;
                }

                DisplayPurchaseInfo();
            }
        }
    }

    // 선택된 map을 산다.
    bool BuyMap()
    {
        int index = (int)imageSelector.SelectedImageIndex;
        if (index > -1 && index < (int)MapController.Map.eCount)
        {
            if (inventoryGameData.mapInfo[index].Diamond > 0)
            {
                if (playerGameData.Diamonds >= inventoryGameData.mapInfo[index].Diamond)
                {
                    playerGameData.Diamonds -= inventoryGameData.mapInfo[index].Diamond;
                    inventoryGameData.mapInfo[index].Enabled = true;

                    return true;
                }
            }
            else if (inventoryGameData.mapInfo[index].Price > 0)
            {
                if (playerGameData.Coins >= inventoryGameData.mapInfo[index].Price)
                {
                    playerGameData.Coins -= inventoryGameData.mapInfo[index].Price;
                    inventoryGameData.mapInfo[index].Enabled = true;

                    return true;
                }
            }
            else if (inventoryGameData.mapInfo[index].AD > 0)
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

    public void OnImageSelectorSelectButtonClicked()
    {
        if(imageSelector)
        {
            int index = (int)imageSelector.SelectedImageIndex;
            // 비활성화 되어 있다면 구매를 한다.
            if (index > -1 && index < (int)MapController.Map.eCount && !inventoryGameData.mapInfo[index].Enabled)
            {
                // 산다.
                if (BuyMap())
                {
                    DisplayPurchaseInfo();
                }
            }
            else
            {
                ChangeMap((MapController.Map)imageSelector.SelectedImageIndex);
            }

            
        }
    }
}
