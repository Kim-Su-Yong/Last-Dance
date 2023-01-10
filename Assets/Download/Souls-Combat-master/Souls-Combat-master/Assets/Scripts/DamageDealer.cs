using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public bool damageOn; // 데미지를 주고 있는지 확인
    public float damageAmount; // 데미지를 얼마나 줄 지 

    public AudioClip[] impactSound; // 맞힌 곳 히트 사운드 출력

    private float lastSoundTime = 0;

    public float GetDamage() // 데미지를 얼마나 입혔는지 확인하는 함수
    {
        return damageAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageOn) return; // 데미지 입힌게 아니라면 반환함
        
        if (other.gameObject.name == "Player") // 만약 게임 오브젝트 이름이 플레이어라면
        {
            if (other.GetComponent<Animator>().GetBool("Hit")) return; // 플레이어가 맞고 있다면
            // other.transform.GetComponentInParent<GirlScript>().RegisterDamage(damageAmount);
        }

        if (SoundInterval() && impactSound.Length > 0)
        {
            lastSoundTime = Time.time;
        }
    }

    public void GreatSwordFiller(GameObject other)
    {
        if (!damageOn) return;

        if (other.gameObject.name == "Player")
        {
            if (other.GetComponent<Animator>().GetBool("Hit")) return;
            //other.transform.GetComponentInParent<GirlScript>().RegisterDamage(damageAmount);
        }

        if (SoundInterval() && impactSound.Length > 0)
        {
            lastSoundTime = Time.time;
        }
    }

    private bool SoundInterval()
    {
        return Time.time > lastSoundTime + 0.5f;
    }

}
