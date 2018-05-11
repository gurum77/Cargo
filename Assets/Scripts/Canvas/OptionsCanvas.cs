using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Controller;
using System.Text;

public class OptionsCanvas : MonoBehaviour {

    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public Button musicButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    SettingGameData settingGameData;

    public Slider soundVolumeSlider;
    public Slider musicVolumeSlider;
    public InputField playerID;
    public Text id;
    public Button createIDButton;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeButtonSprites()
    {
         if(soundButton)
        { 
            if(settingGameData.SoundOnOff > 0)
            {
                soundButton.image.sprite    = soundOnSprite;
            }
            else
            {
                soundButton.image.sprite = soundOffSprite;
            }
        }

        if (musicButton)
        {
            if (settingGameData.MusicOnOff > 0)
            {
                musicButton.image.sprite = musicOnSprite;
            }
            else
            {
                musicButton.image.sprite = musicOffSprite;
            }
        }
    }
    void OnEnable()
    {
        settingGameData = new SettingGameData();
        settingGameData.Load();

        ChangeButtonSprites();
        InitSliders();
        DisplayPlayerID();
    }

    void DisplayPlayerID()
    {
        string idstring = LeaderBoard.GetPlayerID();
        if(idstring.Length > 0)
        {
            playerID.gameObject.SetActive(false);
            createIDButton.gameObject.SetActive(false);

            StringBuilder sb = new StringBuilder();
            sb.Append("ID : ");
            sb.Append(idstring);
            id.text = sb.ToString();
            
        }
    }

    void InitSliders()
    {
        if (settingGameData == null)
            return;
        if (soundVolumeSlider == null)
            return;
        if (musicVolumeSlider == null)
            return;

        soundVolumeSlider.value = settingGameData.SoundVolume;
        musicVolumeSlider.value = settingGameData.MusicVolume;
    }

    public void OnSoundButtonClicked()
    {
        settingGameData.SoundOnOff = settingGameData.SoundOnOff == 0 ? 1 : 0;
        ChangeButtonSprites();
    }

    public void OnMusicButtonClicked()
    {
        settingGameData.MusicOnOff = settingGameData.MusicOnOff == 0 ? 1 : 0;
        ChangeButtonSprites();
    }

    public void CloseButtonClicked()
    {
        settingGameData.Save();
        SceneManager.LoadScene(Define.Scene.Playground);
    }

    public void OnSoundVolumeSliderChanged()
    {
        settingGameData.SoundVolume = soundVolumeSlider.value;

        if(settingGameData.SoundVolume > 0 && settingGameData.SoundOnOff == 0)
        {
            settingGameData.SoundOnOff = 1;
            ChangeButtonSprites();
        }
        else if(settingGameData.SoundVolume == 0 && settingGameData.SoundOnOff == 1)
        {
            settingGameData.SoundOnOff = 0;
            ChangeButtonSprites();
        }
        
    }

    public void OnMusicVolumeSliderChanged()
    {
        settingGameData.MusicVolume = musicVolumeSlider.value;

        if (settingGameData.MusicVolume > 0 && settingGameData.MusicOnOff == 0)
        {
            settingGameData.MusicOnOff = 1;
            ChangeButtonSprites();
        }
        else if (settingGameData.MusicVolume == 0 && settingGameData.MusicOnOff == 1)
        {
            settingGameData.MusicOnOff = 0;
            ChangeButtonSprites();
        }
    }

    public void OnCreateIDButtonClicked()
    {
        if (playerID.text.Length == 0)
            return;

        // 클릭을 하면 ID가 있는지 본다.
        dreamloLeaderBoard lb = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        if (lb == null)
            return;


        if(lb.IsExistPlayerName(playerID.text) == false )
        {
            LeaderBoard.SavePlayerID(playerID.text);
            DisplayPlayerID();
            SceneManager.LoadScene(Define.Scene.Playground);
            return;
        }

        // 있다면 다시 입력
        playerID.textComponent.text = "";
        playerID.textComponent.enabled = false;
        playerID.placeholder.enabled = true;
    }
}
