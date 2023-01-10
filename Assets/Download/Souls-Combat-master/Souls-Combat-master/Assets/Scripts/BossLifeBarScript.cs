using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BossLifeBarScript : MonoBehaviour
{
    private GameObject lifeBarParent;

    public float maxLife = 1000f; // 최대 체력
    public float life = 0f; // 현재 체력
    private float filler = 30f; // 체력을 증가시키기 위한 변수
    private float ghost = 0f; // 안 보이는 체력바
    private int barHeight = 17; // 체력바의 높이
    public Animator bossAnim; // 보스 애니메이터

    [Header("LifeBar")]
    public Image lifeBar; // 체력바
    public Image lifeGhost; // 안 보이는 체력바
    private Animator lifeBarAnim;

    private float lastTime;
    private float waitTime = 1.5f;

    [HideInInspector]
    public bool fillBossLifeBar = false;

    public GameManager gameManager;

    public bool isDie = false;
    public bool isAttack = false;
    public bool isDamaged = false;

    public float beforeHp = 0f;

    private void Start()
    {
        bossAnim = GetComponent<Animator>();
        lifeBarParent = this.transform.parent.gameObject;
        this.GetComponent<CanvasGroup>().alpha = 0;
        life = maxLife;
        lifeBarAnim = lifeBar.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && gameManager.master)
        {
            UpdateLife(-10);
        }
    }

    private void FixedUpdate()
    {
        if(life <= ((maxLife * 50) / 100) && !bossAnim.GetBool("Phase2")) // 체력이 50% 미만이면 2페이즈 시작
        {
            bossAnim.SetTrigger("BeginPhase2");
            bossAnim.SetBool("Phase2", true);
        }

        if (life > ghost && !lifeBarAnim.enabled) // 만약 안 보이는 체력바보다 체력바가 훨씬 더 크다면
        {
            ghost = life;
            lifeGhost.rectTransform.sizeDelta = new Vector2(ghost * filler, barHeight);
        }

        if ((Time.time > lastTime + waitTime) && ghost > life)
        {
            ghost -= 0.1f;
            lifeGhost.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(ghost, life, 8 * Time.deltaTime) * filler, barHeight);
        }

        if (this.GetComponent<CanvasGroup>().alpha == 1 && lifeBarAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
             lifeBarAnim.enabled = false;
    }

    public void UpdateLife(float amount)
    {
        if (IsDead()) return; // 죽으면 반환하지 않음

        if (amount < 0) // 체력이 내려간다면
        {
            lastTime = Time.time;
        }

        life += amount; // 체력에 변화를 주기

        if (life > maxLife) life = maxLife;
        if (life < 0) life = 0;

        if (life == 0 && !IsDead()) // 체력이 0 이 되었다면
        {
            Die();
        }

        lifeBar.rectTransform.sizeDelta = new Vector2(life * filler, barHeight); // 체력바 크기를 계속 조절함
    }

    public void FillBossLifeBar()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        lifeBar.gameObject.SetActive(true);
    }

    private bool IsDead()
    {
        return bossAnim.GetBool("Dead");
    }

    private void Die()
    {
        bossAnim.SetBool("Dead", true);
        bossAnim.SetFloat("Vertical", 0);
        bossAnim.SetFloat("Horizontal", 0);
        StartCoroutine(AfterWin());
        GameManagerScript.isBossDead = true;
    }

    public float GetBossLifeAmount()
    { 
        return life;
    }

    IEnumerator AfterWin()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 offset = new Vector3(0, 0, 1);
        bossAnim.gameObject.GetComponent<CapsuleCollider>().isTrigger = true; 
        CapsuleCollider[] legs = bossAnim.gameObject.GetComponentsInChildren<CapsuleCollider>();
        foreach (CapsuleCollider leg in legs) leg.isTrigger = true; 
        this.gameObject.SetActive(false); 
    }

}
