using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_PlaySound : MonoBehaviour {

    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        
	}

    void PlayEventSound()
    {
        if(audioSource)
        {
            audioSource.Play();
        }
    }

    void StopEventSound()
    {
        if(audioSource)
        {
            audioSource.Stop();
        }
    }
}
