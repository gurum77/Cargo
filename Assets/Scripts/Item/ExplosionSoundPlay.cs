using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 폭발 소리 재생
public class ExplosionSoundPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        GameController.Instance.mySoundManager.PlaySound(DigitalRuby.SoundManagerNamespace.MySoundManager.Sound.eExplosion);
    }
}
