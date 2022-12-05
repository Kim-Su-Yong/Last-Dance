using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Transform model;

    private Animator anim;
    public Transform player;

    //public AudioClip[] takeDamageSound;
    //public BossLifeBarScript bossLifeScript;

    //public GameObject hitCounterParent;

    private float rotationSpeed = 6;

    private float lastDamageTakenTime = 0;

    // Hit Counter
    //private int hit = 0; 
    //private int currentHit = 0;
    //public Text hitCounterText;
    //public Text hitAdderText;

    void Start()
    {
        anim = model.GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetBool("Dead")) return;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") &&
            anim.GetCurrentAnimatorStateInfo(1).IsName("None") ||
            !anim.GetBool("CanRotate"))
        {
           //Vector3 rotationOffset = player.transform.position - model.position;
            //rotationOffset.y = 0;
            //float lookDirection = Vector3.SignedAngle(model.forward, rotationOffset, Vector3.up);
            //anim.SetFloat("LookDirection", lookDirection);
        }
        else if (!anim.GetBool("Attacking") && anim.GetBool("CanRotate"))
        {
            var targetRotation = Quaternion.LookRotation(player.transform.position - model.transform.position);

            // Smoothly rotate towards the target point.
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, 
                targetRotation, rotationSpeed * Time.deltaTime);
        }

        model.transform.eulerAngles = new Vector3(0, model.transform.eulerAngles.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && other.gameObject.GetComponentInParent<Animator>().GetBool("Attacking") 
            && !anim.GetBool("Attacking") && DamageInterval() && !anim.GetBool("Dead"))
        {
            lastDamageTakenTime = Time.time;
            //CreateAndPlay(takeDamageSound[UnityEngine.Random.Range(0, takeDamageSound.Length)], 2);
            StopAllCoroutines();
            //StartCoroutine(ShowHitCounter()); 
            if (!anim.GetBool("TakingDamage") && !anim.GetBool("Attacking") && anim.GetBool("NotAttacking")) 
                anim.SetTrigger("TakeDamage");

            //GameObject blood = Instantiate(bloodPrefab, bloodPos.position, Quaternion.identity);
            //blood.transform.LookAt(player.position);
            //Destroy(blood, 0.2f);
        }
    }

    public void RegisterPlayerSwordFillDamage()
    {
        if (!anim.GetBool("Attacking") && DamageInterval() && !anim.GetBool("Dead"))
        {
            lastDamageTakenTime = Time.time;
            //CreateAndPlay(takeDamageSound[UnityEngine.Random.Range(0, takeDamageSound.Length)], 2);
            StopAllCoroutines(); 
            //StartCoroutine(ShowHitCounter()); 
            if (!anim.GetBool("TakingDamage") && !anim.GetBool("Attacking") && anim.GetBool("NotAttacking"))
                anim.SetTrigger("TakeDamage");

            //GameObject blood = Instantiate(bloodPrefab, bloodPos.position, Quaternion.identity);
            //blood.transform.LookAt(player.position);
            //Destroy(blood, 0.2f);
        }
    }

    //public void SwordHit(int hit)
    //{
    //    this.hit = hit;
    //    if (hit == 0)
    //        ClearCurrentHit();
    //}

    //public void ClearCurrentHit()
    //{
    //    this.currentHit = 0;
    //}

    //public void HitManager() 
    //{
    //    if (currentHit == 0 && hit == 4) 
    //    {
    //        hitCounterText.text = "1 Hit";
    //        hitAdderText.text = "+50%";
    //        bossLifeScript.UpdateLife(-1.5f);
    //        return;
    //    }

    //    currentHit++; 

    //    if (hit == 1) currentHit = 1; 

    //    if (currentHit == 1 && hit == 1)
    //    {
    //        hitCounterText.text = "1 Hit";
    //        hitAdderText.text = " ";
    //        bossLifeScript.UpdateLife(-1);
    //    }
    //    else if (currentHit == 1 && hit == 4) 
    //    {
    //        hitCounterText.text = "2 Hits";
    //        hitAdderText.text = "+75%";
    //        bossLifeScript.UpdateLife(-1.75f);
    //    }
    //    else if (currentHit == 0 && hit == 4) 
    //    {
    //        hitCounterText.text = "1 Hit";
    //        hitAdderText.text = "+50%";
    //        bossLifeScript.UpdateLife(-1.5f);
    //    }

    //    if (currentHit == 2 && hit == 2) 
    //    {
    //        hitCounterText.text = "2 Hits";
    //        hitAdderText.text = "+50%";
    //        bossLifeScript.UpdateLife(-1.5f);
    //    }
    //    else if (currentHit == 2 && hit == 4)
    //    {
    //        hitCounterText.text = "2 Hits";
    //        hitAdderText.text = "+75%";
    //        bossLifeScript.UpdateLife(-1.75f);
    //    }

    //    if (currentHit == 3 && hit == 3) 
    //    {
    //        hitCounterText.text = "3 Hits";
    //        hitAdderText.text = "+75%";
    //        bossLifeScript.UpdateLife(-1.75f);
    //    }
    //    else if (currentHit == 3 && hit == 4) 
    //    {
    //        hitCounterText.text = "3 Hits";
    //        hitAdderText.text = "+100%";
    //        bossLifeScript.UpdateLife(-2f);
    //    }

    //    if (currentHit == 4)
    //    {
    //        hitCounterText.text = "4 Hits";
    //        hitAdderText.text = "+150%";
    //        bossLifeScript.UpdateLife(-2.5f);
    //    }
    //}

    private bool DamageInterval()
    {
        return (Time.time > lastDamageTakenTime + 0.7f);
    }

    //IEnumerator ShowHitCounter()
    //{
    //    HitManager();
    //    hitCounterParent.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    hitCounterParent.SetActive(false);
    //}

    private void CreateAndPlay(AudioClip clip, float destructionTime, float volume = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, destructionTime);
    }

}
