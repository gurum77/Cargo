using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelAlert : MonoBehaviour {

    public AudioSource alertSound;
    Animator ani;
    float playingTime;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        playingTime += Time.deltaTime;

        // 3초간 경고
        if(playingTime > 3.0f)
        {
            
            if (alertSound)
            {
                alertSound.volume -= Time.deltaTime;
                if(alertSound.volume <= 0)
                {
                    alertSound.Stop();
                    gameObject.SetActive(false);
                }
            }
        }
	}

    void OnEnable()
    {
        ani = GetComponentInChildren<Animator>();
        if(ani)
        {
            ani.SetTrigger(Define.Trigger.Alert_Twinkling);
            playingTime = 0;
        }

        if(alertSound)
        {
            alertSound.volume = 1.0f;
            alertSound.Play();
        }
    }

    
}
