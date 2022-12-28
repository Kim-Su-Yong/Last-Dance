using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // 기초적인 Base Enemy 코드
    public int routine;
    public float timer;
    public float time_routine;
    public Animator ani;
    public Quaternion angle;
    public float degree;
    public GameObject target;
    public bool isAttack;
    public RangeBoss range;
    public float speed;
    public GameObject[] hit;
    public int hit_select;

    // 화염 방사
    //public bool isFlame;
    //public List<GameObject> flamePool = new List<GameObject>();
    //public GameObject flame;
    //public GameObject head;
    //private float timer2;

    // 점프 공격
    //public float jumpDistance;
    //public bool directionSkill;

    // 파이어볼
    public GameObject fireBall;
    public GameObject point;
    public List<GameObject> fireBallPool = new List<GameObject>();

    // 기초적인 보스 셋팅
    public int phase = 1;
    public float hpMin;
    public float hpMax;
    //public Image hpBar;
    //public AudioSource music;
    public bool isDie;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
    }

    public void BossBehaviour()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 15)
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            point.transform.LookAt(target.transform.position);
            //music.enabled = true;

            if (Vector3.Distance(transform.position, target.transform.position) > 2 && !isAttack)
            {
                switch (routine)
                {
                    // 걷기
                    case 0:
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2 );
                        ani.SetBool("isWalk", true);

                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * speed * Time.deltaTime);
                        }

                        ani.SetBool("isAttack", false);
                        timer += 1 * Time.deltaTime;

                        if (timer > time_routine)
                        {
                            routine = Random.Range(0, 5);
                            timer = 0;
                        }
                        break;

                    // 화염방사
                    //case 1:
                    //    ani.SetBool("isWalk", false);
                    //    ani.SetBool("isAttack", true);
                    //    ani.SetFloat("Skills", 1f);
                    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                    //    range.GetComponent<CapsuleCollider>().enabled = false;
                    //    break;

                    // 점프 공격
                    //case 2:
                    //    if(phase==2)
                    //    {
                    //        jumpDistance += 1 * Time.deltaTime;
                    //        ani.SetBool("isWalk", false);
                    //        ani.SetBool("isAttack", true);
                    //        ani.SetFloat("Skills", 0);
                    //        hit_select = 3;
                    //        rango.GetComponent<CapsuleCollider>().enabled = false;

                    //        if(directionSkill)
                    //        {
                    //            if(jumpDistance <1f)
                    //            {
                    //                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                    //            }
                    //            transform.Translate(Vector3.forward * 8 * Time.deltaTime);
                    //        }
                    //        else
                    //        {
                    //            routine = 0;
                    //            time_routine = 0;
                    //        }
                    //    }
                    //    break;

                    // 파이어볼
                    case 3:
                        if (phase == 2)
                        {
                            ani.SetBool("isWalk", false);
                            ani.SetBool("isAttack", true);
                            ani.SetFloat("Skills", 1f);
                            range.GetComponent<CapsuleCollider>().enabled = false;
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        }
                        else
                        {
                            routine = 0;
                            time_routine = 0;
                        }
                        break;
                }
            }
        }
    }

    public void FinalAni()
    {
        routine = 0;
        ani.SetBool("isAttack", false);
        isAttack = false;
        range.GetComponent<CapsuleCollider>().enabled = true;
        //isFlame = true;
        //jumpDistance = 0;
        //directionSkill = false;
    }

    //public void DirectionAttackStart()
    //{
    //    directionSkill = true;
    //}

    //public void DirectionAttackFinal()
    //{
    //    directionSkill = false;
    //}

    public void ColliderWeaponTrue()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = false;
    }

    //public GameObject GetFlame()
    //{
    //    for (int i = 0; i < flamePool.Count; i++)
    //    {
    //        if (!flamePool[i].activeInHierarchy)
    //        {
    //            flamePool[i].SetActive(true);
    //            return flamePool[i];
    //        }
    //    }
    //    GameObject obj = Instantiate(flame, head.transform.position, head.transform.rotation) as GameObject;
    //    flamePool.Add(obj);
    //    return obj;
    //}

    //public void FlameSkill()
    //{
    //    timer2 += 1 * Time.deltaTime;
    //    if (timer2 > 0.1f)
    //    {
    //        GameObject obj = GetFlame();
    //        obj.transform.position = head.transform.position;
    //        obj.transform.rotation = head.transform.rotation;
    //        timer2 = 0;
    //    }
    //}

    //public void StartFlame()
    //{
    //    isFlame = true;
    //}

    //public void StopFlame()
    //{
    //    isFlame = false;
    //}

    public GameObject GetFireBall()
    {
        for (int i = 0; i < fireBallPool.Count; i++)
        {
            if (!fireBallPool[i].activeInHierarchy)
            {
                fireBallPool[i].SetActive(true);
                return fireBallPool[i];
            }
        }
        GameObject obj = Instantiate(fireBall, point.transform.position, point.transform.rotation) as GameObject;
        fireBallPool.Add(obj); 
        return obj;
    }

    public void FireBallSkill()
    {
        GameObject obj = GetFireBall();
        obj.transform.position = point.transform.position;
        obj.transform.rotation = point.transform.rotation;
    }

    public void Alive()
    {
        // 2페이즈 시작
        if (hpMin < 500)
        {
            phase = 2;
            time_routine = 1;
        }

        BossBehaviour();

        //if (isFlame)
        //{
        //    FlameSkill();
        //}
    }

    void Update()
    {
        //hpBar.fillAmount = hpMin / hpMax;
        if (hpMin > 0)
        {
            Alive();
        }
        else
        {
            if (!isDie)
            {
                ani.SetTrigger("Die");
                //music.enabled = false;
                isDie = true;
            }
        }
    }
}
