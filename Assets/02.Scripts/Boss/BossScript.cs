using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public Transform model; // 기본적으로 갖고 온 Transform 이 맞지 않아서 선언함
    public Transform greatSword; // 보스의 공격 위치

    private Animator anim;
    public Transform player;

    public AudioClip[] takeDamageSound;
    public BossLifeBarScript bossLife;

    public GameObject bloodPrefab;
    public Transform bloodPos;

    private int hit = 0;
    private int currentHit = 0;

    private float rotationSpeed = 6;
    private float lastDamageTakenTime = 0;

    void Start()
    {
        anim = model.GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetBool("Dead")) return; // 보스가 죽으면 끝

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle")
            && anim.GetCurrentAnimatorStateInfo(1).IsName("None") || 
            !anim.GetBool("CanRotate")) // 현재 애니메이션 상태가 이렇다면
        {
            // 보스의 시작 지점과 바라보는 방향을 지정해줌
            Vector3 rotationOffset = player.transform.position - model.position;
            rotationOffset.y = 0;
            float lookDirection = Vector3.SignedAngle(model.forward, rotationOffset, Vector3.up);
            anim.SetFloat("LookDirection", lookDirection);
        }
        else if (!anim.GetBool("Attacking") && anim.GetBool("CanRotate"))
        {
            var targetRotation = Quaternion.LookRotation(player.transform.position - model.transform.position);

            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, 
                targetRotation, rotationSpeed * Time.deltaTime); // 부드럽게 돌아가도록 회전 보간을 넣음
        }

        model.transform.eulerAngles = new Vector3(0, model.transform.eulerAngles.y, 0);
    }

    private void GreatSwordCollider(bool b) // 검 활성화
    {
        greatSword.GetComponent<BoxCollider>().isTrigger = !b;
    }

    private void OnTriggerEnter(Collider other) // 보스가 공격 시
    {
        if (other.gameObject.tag == "Sword" &&
            other.gameObject.GetComponentInParent<Animator>().GetBool("Attacking")
            && !anim.GetBool("Attacking") && DamageInterval() && !anim.GetBool("Dead"))
        {
            lastDamageTakenTime = Time.time;
            CreateAndPlay(takeDamageSound[UnityEngine.Random.Range(0, takeDamageSound.Length)], 2);
            StopAllCoroutines();

            if (!anim.GetBool("TakingDamage") && !anim.GetBool("Attacking") && anim.GetBool("NotAttacking"))
                anim.SetTrigger("TakeDamage");

            GameObject blood = Instantiate(bloodPrefab, bloodPos.position, Quaternion.identity);
            blood.transform.LookAt(player.position);
            Destroy(blood, 0.2f);
        }
    }

    public void RegisterPlayerSwordFillDamage()
    {
        if (!anim.GetBool("Attacking") && DamageInterval() && !anim.GetBool("Dead"))
        {
            lastDamageTakenTime = Time.time;
            CreateAndPlay(takeDamageSound[UnityEngine.Random.Range(0, takeDamageSound.Length)], 2);
            StopAllCoroutines();
            if (!anim.GetBool("TakingDamage") && !anim.GetBool("Attacking") && anim.GetBool("NotAttacking"))
                anim.SetTrigger("TakeDamage");

            GameObject blood = Instantiate(bloodPrefab, bloodPos.position, Quaternion.identity);
            blood.transform.LookAt(player.position);
            Destroy(blood, 0.2f);
        }
    }
    public void HitByPlayer(int hit)
    {
        this.hit = hit;
        if (hit == 0)
            ClearCurrentHit();
    }

    public void ClearCurrentHit()
    {
        this.currentHit = 0;
    }

    public void HitManager()
    {
        if (currentHit == 0 && hit == 4)
        {
            bossLife.UpdateLife(-1.5f);
            return;
        }

        currentHit++;

        if (hit == 1) currentHit = 1;

        if (currentHit == 1 && hit == 1)
        {
            bossLife.UpdateLife(-1);
        }
        else if (currentHit == 1 && hit == 4)
        {
            bossLife.UpdateLife(-1.75f);
        }
        else if (currentHit == 0 && hit == 4)
        {
            bossLife.UpdateLife(-1.5f);
        }

        if (currentHit == 2 && hit == 2)
        {
            bossLife.UpdateLife(-1.5f);
        }
        else if (currentHit == 2 && hit == 4)
        {
            bossLife.UpdateLife(-1.75f);
        }

        if (currentHit == 3 && hit == 3)
        {
            bossLife.UpdateLife(-1.75f);
        }
        else if (currentHit == 3 && hit == 4)
        {

            bossLife.UpdateLife(-2f);
        }

        if (currentHit == 4)
        {
            bossLife.UpdateLife(-2.5f);
        }
    }

    private bool DamageInterval() // 데미지 주는 타격 간격
    {
        return (Time.time > lastDamageTakenTime + 0.7f);
    }

    private void CreateAndPlay(AudioClip clip, float destructionTime, float volume = 1f) // 보스 오디오 소스
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, destructionTime);
    }

}