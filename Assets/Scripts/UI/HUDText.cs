using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDText : MonoBehaviour {
    float timeFromStart;    // 올라가기 시작한 시간
    
    public GameObject target;
    public float speedToUp; // 올라가는 속도
    public float timeToUp;  // 올라가는 시간

    Text text;
    Vector3 position;


	// Use this for initialization
	void Start () {

        // 타겟에서 시작 위치를 찾아 온다.
        if(target)
        {
            Vector3 targetPosition = target.transform.position;
            // 머리 위에서 시작한다.
            Renderer renderer = target.GetComponentInChildren<Renderer>();
            if (renderer)
            {
                targetPosition.y += renderer.bounds.size.z;
            }

            transform.position = Camera.main.WorldToScreenPoint(targetPosition);
            position = transform.position;

           
        }

        // text 객체
        text = GetComponentInChildren<Text>();
	}
	// Update is called once per frame
	void Update ()
    {

        position.y += (Time.deltaTime * speedToUp);
        transform.position = position;

        // 올라가는 시간 증가
        timeFromStart += Time.deltaTime;
        if (timeFromStart > timeToUp)
        {
            GameObject.DestroyObject(gameObject);
        }
	}
}
