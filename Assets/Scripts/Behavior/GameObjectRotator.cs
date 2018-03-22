using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트를 계속 회전한다.
public class GameObjectRotator : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Quaternion rotation = transform.rotation;
        Vector3 ang = rotation.eulerAngles;
        ang.y += Time.deltaTime * speed;
        if (ang.y > 360)
            ang.y = 0;
        transform.rotation = Quaternion.Euler(ang);
	}

    void FixedUpdate()
    {
      
    }

}
