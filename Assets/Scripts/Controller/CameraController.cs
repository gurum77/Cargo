using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player를 지정된 방향으로 보면서 따라가는 카메라
/// </summary>
public class CameraController : MonoBehaviour {

    public GameObject player;
    public float followingSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = player.transform.position;
        pos.y += 10;
        pos.z -= 2;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followingSpeed);

	}
}
