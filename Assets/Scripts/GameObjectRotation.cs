using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRotation : MonoBehaviour {

    public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion rotation = transform.rotation;
        Vector3 ang = rotation.eulerAngles;
        ang.y += (Time.deltaTime * rotationSpeed);
        rotation = Quaternion.Euler(ang);
        transform.rotation = rotation;
	}
}
