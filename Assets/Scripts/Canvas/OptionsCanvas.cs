using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsCanvas : MonoBehaviour {

    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        if(soundButton)
        { 
            if(GetVolumn() > 0)
            {
                soundButton.image.sprite    = soundOnSprite;
            }
            else
            {
                soundButton.image.sprite = soundOffSprite;
            }
        }
    }

    int GetVolumn()
    {
        return 1;   
    }


    public void OnSoundButtonClicked()
    {
        
    }
}
