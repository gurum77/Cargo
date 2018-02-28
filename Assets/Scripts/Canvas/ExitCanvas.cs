using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCanvas : MonoBehaviour {

    public GameObject items;
	// Use this for initialization
	void Start () {
		
	}
	
    void Update()
    {

    }
	
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void OnResumeButtonClicked()
    {
        if(items)
        {
            items.SetActive(false);
        }
    }



}
