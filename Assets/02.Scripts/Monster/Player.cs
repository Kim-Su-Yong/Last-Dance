using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public float health = 100f;

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool fDown;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady;
    bool isDamage;
    public bool isDead;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    [SerializeField]
    MeshRenderer[] meshes;

    //public static Player player;
    //Weapon equipWeapon;

    float fireDelay = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshes = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Dodge();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");    
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
    }
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;
        // ★ wDown(!대시) 사용법 1
        //if(wDown)
        //    transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        //else
        //    transform.position += moveVec * speed * Time.deltaTime;

        // ★ wDown(!대시) 사용법 2
        transform.position += moveVec * speed *(wDown? 0.3f: 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }
    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Attack()
    {
        fireDelay += Time.deltaTime;
        isFireReady = 1.0f < fireDelay;
        //isFireReady = true;

        if (Input.GetKeyDown(KeyCode.Mouse0) && isFireReady && !isDodge && !isSwap)
        {

            Weapon.equipWeapon.Use();
            anim.SetTrigger("doSwing");
            
            fireDelay = 0;
        }
    }
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 2f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            if (!isDamage && !isDead)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);

                StartCoroutine(OnDamage());
            }
            if (health <= 0 && !isDead)
            {
                PlayerDie();
            }
        }
    }
    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.yellow;
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }
    }
    void PlayerDie()
    {
        isDead = true;
        anim.SetTrigger("doDie");
        float DeadTime = Time.deltaTime;
        if (DeadTime >= 3)
        {
            GameObject player = GetComponent<GameObject>();
            player.SetActive(false);
        }

    }

}
