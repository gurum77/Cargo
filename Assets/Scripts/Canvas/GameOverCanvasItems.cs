using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// gameover canvas items은 활성화 될때 adbutton을 살려줘야 한다.
public class GameOverCanvasItems : MonoBehaviour {

    public Button adButton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        if(adButton)
        {
            adButton.gameObject.SetActive(true);
        }
    }
}
