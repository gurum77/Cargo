using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player를 지정된 방향으로 보면서 따라가는 카메라
/// </summary>
public class CameraController : MonoBehaviour {

    public GameObject player;
    public float followingSpeed;
    public bool skyView = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(player)
        {
            Vector3 pos = player.transform.position;
            if (skyView)
            {
                pos.y += 10;
                pos.z -= 2;
            }
            else
            {
                pos.y += 2;
                pos.z -= 2;
            }


            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followingSpeed);

            // rotation
            Quaternion targetRotation = skyView == true ? Quaternion.Euler(70, 0, 0) : Quaternion.Euler(40, 0, 0);
            Quaternion curRotation = transform.rotation;
            transform.rotation = Quaternion.Lerp(curRotation, targetRotation, Time.deltaTime * followingSpeed);
        }
        
	}
}
