using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rock item 스크립트
public class Rock : MonoBehaviour {

    // 기본 체력
    public int health;

    float firstPosition;
    int totalDamage;  // 총 damage
    Animator ani;
    public ParticleSystem damageEffect;
    public ParticleSystem destroyEffect; 

	// Use this for initialization
	void Start () {

        // 최초 위치를 보관
        firstPosition = transform.position.y;

        ani = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // damage를 준다.
    // damage를 줄때마다 에너지가 줄어든다.
    public void AddDamage(int power)
    {
        totalDamage += power;
        
        // damage를 받을때 마다 y position을 조정한다.
        AdjustYPosition();

        // animation 발동
        if(ani)
        {
            ani.SetTrigger(Define.Trigger.Rock_Damage);
        }

        if(damageEffect)
        {
            damageEffect.Play();
        }

        if(destroyEffect)
        {
            // 남은 채력이 없거나 반이하로 남았을때
            if (!IsRemainHealth() || (totalDamage > health / 2 && !destroyEffect.isPlaying))
            {
                destroyEffect.Play();
            }
        }
 
    }

    // 체력이 남아 있는지?
    public bool IsRemainHealth()
    {
        return health > totalDamage ? true : false;
    }

    // Y scale을 조정한다.
    void AdjustYPosition()
    {
        Vector3 position = transform.position;
        position.y = firstPosition - (firstPosition * (float)((float)totalDamage / (float)health));
        transform.position = position;
    }


    void OnEnable()
    {

        totalDamage = 0;

        
    }
}
