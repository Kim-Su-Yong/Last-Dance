using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public LifeBarScript lifeBarScript;
    private Transform player;
    private float speed = 50;
    private float turn = 20;
    private Vector3 offset;

    private float distance;
    private Rigidbody rb;

    private bool chase = true;

    private float lastTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector3(0, 1f, 0);
        rb = this.GetComponent<Rigidbody>();
        lifeBarScript = GameObject.Find("Canvas").transform.Find("LifeBar Parent").GetChild(0).GetComponent<LifeBarScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = (player.transform.position - this.transform.position).sqrMagnitude;

        rb.velocity = transform.forward * speed; // 발사체에 속력을 가한다

        if(distance > 2 && chase) // 플레이어를 통과하지 못했다면
        {
            Quaternion targetRotation = Quaternion.LookRotation((player.position + offset) - transform.position);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
        } 
        else
        {
            chase = false; // 더 이상 플레이어를 추적하지 않음
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!TimeInterval()) return;
        lastTime = Time.time;
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity); // 폭발
        GameObject pos = GameObject.FindGameObjectWithTag("SoundManager").gameObject; // 폭발 위치

        if(other.gameObject.tag == "Player" && !player.GetComponent<Animator>().GetBool("Hit")) // 플레이어를 쳤을 때
        {
            // lifeBarScript.StartBleeding();
        }

        Destroy(this.gameObject, 0.1f);
    }

    private bool TimeInterval()
    {
        return Time.time > lastTime + 0.5f;
    }

}
