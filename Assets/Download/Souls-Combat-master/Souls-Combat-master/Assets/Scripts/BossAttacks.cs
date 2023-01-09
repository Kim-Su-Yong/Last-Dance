using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BossAttacks : MonoBehaviour
{
    [Header("Control")]
    public bool AI; // 기본적으로 AI 가 조종할 수 있음
    public bool debug; // 디버그용 선언

    [Header("References")]
    public Transform model;
    public Transform player;
    public BoxCollider leftFootCollider; // 발차기용 왼발 콜라이더
    public Transform spellPosition; // 마법 발사 위치 지정
    public Transform impactPosition; // 공격에 임팩트를 주기 위한 위치 지정
    private Animator playerAnim; // 플레이어 애니메이션에 반응하기 위해 지정함
    public DamageDealer greatSword; // 기본 공격용 칼
    public GameManager gameManager; // 게임 매니저에 연동해야함

    [Header("Attacks")]
    public GameObject earthShatterPrefab;
    public GameObject magicSwordFromSky;
    public GameObject spell;
    public GameObject auraMagic;
    public GameObject screamMagic;
    public GameObject magicFarSword;
    public GameObject impactPrefab;

    private Animator anim;

    [Header("AI Manager")]
    public float nearValue;
    public float farValue;
    public float chillTime;
    private string action;
    private float lastActionTime;
    private float distance;
    private float chillDirection;
    private bool phase2; // 2페이즈가 있음!
    private bool canBeginAI; // AI 가 조종하는 것을 원칙으로 함
    private int lastAttack;

    private bool slowDown;
    private string actionAfterSlowDown;


    private void Start()
    {
        anim = model.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();

        Vector3 size = new Vector3(0.00075f, 0.0004f, 0.014f);
        Vector3 center = new Vector3(0f, 0f, 0.007f);
        SetGreatSwordSize(size, center); // 검의 크기를 조절하기 위한 설정
    }

    private void Update()
    {
        distance = Vector3.Distance(model.transform.position, player.transform.position);

        if (distance < 20 && !anim.GetBool("Equipped")) // 플레이어가 보스와 거리가 가까워졌다면
        {
            anim.SetTrigger("DrawSword"); // 검을 꺼내고
            StartCoroutine(StartAI()); // AI 를 스타트시킴
        }

        if (!anim.GetBool("Equipped")) return; // 플레이어가 없다면 대기

        if (!canBeginAI) return;

        if (AI && !playerAnim.GetBool("Die"))
        {
            AI_Manager(); // AI 상태고 플레이어가 아직 죽지 않았다면
        }
        else if (gameManager.master)
        {
            DebugAttack(); // 디버그용...
        }
        else
        {
            anim.SetBool("GameEnd", true); // 끝났다면 행동을 중지하게끔 함
            anim.SetBool("CanRotate", false);
        }

        greatSword.damageOn = anim.GetBool("Attacking");

        phase2 = anim.GetBool("Phase2"); // 2페이즈 시작

    }

    IEnumerator StartAI()
    {
        yield return new WaitForSeconds(4);
        canBeginAI = true;
    }

    private void FarAttack() // 만약 거리가 멀다면
    {
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);

        int rand = 0; // 랜덤값을 부여해서
        do
        {
            if (!anim.GetBool("Phase2")) rand = Random.Range(0, 7);
            if (anim.GetBool("Phase2")) rand = Random.Range(0, 8); // 2페이즈 일 경우 공격 패턴 1개 추가
        } 
        while (rand == lastAttack); // do while 문으로 랜덤한 공격 시전
        lastAttack = rand;

        if (anim.GetBool("Phase2") && Random.Range(0, 2) == 0)
        {
            anim.SetTrigger("Spell"); // 파이어볼
        }

        switch (rand)
        {
            case 0:
                anim.SetTrigger("CastMagicSwords"); // 하늘에서 마법 검 소환
                break;
            case 1:
                anim.SetTrigger("Casting"); // 지면 강타
                break;
            case 2:
                anim.SetTrigger("Dash");
                break;
            case 3:
                anim.SetTrigger("DoubleDash");
                break;
            case 4:
                anim.SetTrigger("Spell"); // 파이어볼
                break;
            case 5:
                anim.SetTrigger("Scream");
                break;
            case 6:
                anim.SetTrigger("Fishing"); // 먼 거리에서 마법 검 공격
                break;
            case 7:
                anim.SetTrigger("SuperSpinner"); // 2페이즈 전용 공격기
                break;
            default:
                break;
        }

        action = "Wait"; // 공격이 재실행되지 않도록 함
    }

    private void NearAttack() // 만약 거리가 가깝다면
    {
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);

        int rand = 0;
        do
        {
            if (!anim.GetBool("Phase2")) rand = Random.Range(0, 10);
            if (anim.GetBool("Phase2")) rand = Random.Range(0, 13);
        } 
        while (rand == lastAttack); // 마찬가지로 랜덤값 부여
        lastAttack = rand;

        switch (rand)
        {
            case 0:
                anim.SetTrigger("DoubleDash"); // 두 번 공격
                break;
            case 1:
                anim.SetTrigger("Dash");
                break;
            case 2:
                anim.SetTrigger("SpinAttack"); // 회전 공격
                break;
            case 3:
                anim.SetTrigger("Combo"); // 연타 공격
                break;
            case 4:
                anim.SetTrigger("Casting"); // 지면 강타
                break;
            case 5:
                anim.SetTrigger("Combo1"); // 다른 연타 공격
                break;
            case 6:
                anim.SetTrigger("Spell"); // 파이어볼
                break;
            case 7:
                anim.SetTrigger("AuraCast"); // 오오라 캐스팅 공격
                break;
            case 8:
                anim.SetTrigger("ForwardAttack");
                break;
            case 9:
                anim.SetTrigger("Scream");
                break;
                // 해당 위치부터 2페이즈 전용 공격기
            case 10:
                anim.SetTrigger("Impact");
                break;
            case 11:
                anim.SetTrigger("Strong");
                break;
            case 12:
                anim.SetTrigger("JumpAttack");
                break;
            default:
                break;
        }

        action = "Wait"; // 공격이 재실행되지 않도록 함

    }

    private void SlowBossDown() // 천천히 다가가게끔 움직임 설정
    {
        if (anim.GetFloat("Vertical") <= 0.4f)
        {
            slowDown = false;
            if (actionAfterSlowDown == "CallNextMove")
            {
                action = "Wait";
                anim.SetFloat("Vertical", 0);
                anim.SetFloat("Horizontal", 0);
                StartCoroutine(WaitAfterNearMove());
            }
            else if (actionAfterSlowDown == "FarAttack")
            {
                action = "FarAttack";
            }
            else
            {
                Debug.LogError("Not supposed to be here");
            }
        }
        else
        {
            anim.SetFloat("Vertical", Mathf.Lerp(anim.GetFloat("Vertical"), 0, 1 * Time.deltaTime));
        }
    }

    IEnumerator WaitAfterNearMove() // 근접했을 때 움직임 설정
    {
        slowDown = false;
        action = "Wait";
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);
        float maxWaitTime = 6; // 6초 이상 유후 상태가 되지 않도록 설정
        float possibility = 2; // 2가지 모드
        if (anim.GetBool("Phase2")) // 2페이즈 시작 시 변경값 부여
        {
            maxWaitTime = 5.5f;
            possibility = 2;
        }
        float waitTime;
        float decision = Random.Range(0, possibility);
        if (decision == 0) waitTime = Random.Range(2.5f, maxWaitTime);
        else waitTime = 0;
        yield return new WaitForSeconds(waitTime);
        action = "NearAttack";

        CallNextMove(); // 다음 움직임
    }

    private void MoveToPlayer() // 플레이어에게 전진
    {
        anim.SetFloat("Horizontal", 0);

        float speedValue = distance / 15; // 거리가 가까워질수록 느리게 움직이지만
        if (speedValue > 1) speedValue = 1; // 스피드 값이 1 이하일 경우 강제로 1 로 고정시킴

        if (slowDown)
        {
            SlowBossDown();
            return;
        }

        if (distance < nearValue) // 거리가 멀다면
        {
            actionAfterSlowDown = "CallNextMove";
            slowDown = true;
        }
        else if (Time.time - lastActionTime > chillTime) // 현재 시간에서 플레이어가 거리가 먼 채로 오래 있었다면
        {
            actionAfterSlowDown = "FarAttack";
            slowDown = true;
        }
        else
        {
            anim.SetFloat("Vertical", speedValue);
        }
    }

    private void WaitForPlayer() // 플레이어 기다리기
    {
        anim.SetFloat("Horizontal", chillDirection);
        anim.SetFloat("Vertical", 0);

        if ((distance <= nearValue && Time.time - lastActionTime > chillTime) && !phase2)
        {
            CallNextMove();
        }
        else

        if ((distance > farValue && Time.time - lastActionTime > chillTime) && !phase2)
        {
            FarAttack();
        }
        else

        if ((Time.time - lastActionTime > chillTime) || phase2 && Time.time - lastActionTime > chillTime)
        {
            int rand = Random.Range(0, 3);

            if (rand % 2 == 0)
            {
                NearAttack();
            }
            else if (rand % 2 == 1)
            {
                FarAttack();
            }
        }

    }

    private void AI_Manager()
    {
        if (action == "Wait" || anim.GetBool("Dead") || anim.GetBool("Transposing")) return;

        if (action == "Move")
        {
            MoveToPlayer();
        }

        if (action == "WaitForPlayer")
        {
            WaitForPlayer();
        }

        if (action == "FarAttack")
        {
            if (!anim.GetBool("TakingDamage"))
                FarAttack();
        }

        if (action == "NearAttack")
        {
            if (!anim.GetBool("TakingDamage"))
            {
                NearAttack();
            }
        }
    }

    public void CallNextMove()
    {
        lastActionTime = Time.time;

        if (distance >= farValue && !anim.GetBool("Dead"))
        {
            action = "Move";
        }
        else if (distance > nearValue && distance < farValue && !anim.GetBool("Dead"))
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) chillDirection = -0.5f;
            if (rand == 1) chillDirection = 0.5f;
            action = "WaitForPlayer";
        }
        else if (distance <= nearValue && !anim.GetBool("Dead"))
        {
            action = "NearAttack";
        }
    }

    private bool IsBossTakingDamage() // 데미지 입고 있는지 확인하는 변수
    {
        return !anim.GetCurrentAnimatorStateInfo(2).IsName("none");
    }

    #region 디버그용(확인하고 싶다면 여시오)

    private void DebugAttack()
    {
        anim.SetFloat("Vertical", 0);

        if (Input.GetKeyDown(KeyCode.B))
        {
            anim.SetTrigger("AuraCast");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("Spell");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetTrigger("Impact");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("SpinAttack");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("Casting");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetTrigger("Strong");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            anim.SetTrigger("CastMagicSwords");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            anim.SetTrigger("SuperSpinner");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            anim.SetTrigger("JumpAttack");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("ForwardAttack");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("Scream");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Fishing");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("Combo");
        }

    }

    #endregion

    #region 마법들
    public void SpawnEarthShatter() // 지면 강타
    {
        Vector3 bossPos = model.transform.position;
        Vector3 bossDirection = model.transform.forward;
        Quaternion bossRotation = model.transform.rotation;
        float spawnDistance = 3;

        Vector3 spawnPos = bossPos + bossDirection * spawnDistance;
        GameObject earthShatter = Instantiate(earthShatterPrefab, spawnPos, Quaternion.identity);
        earthShatter.transform.rotation = bossRotation;
        Destroy(earthShatter, 4);
    }

    public void Scream() // 포효
    {
        GameObject scream = Instantiate(screamMagic, model.transform.position, Quaternion.identity);
        scream.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void SwordsFromSkyAttack() // 하늘에서 마법 검 소환
    {
        StartCoroutine(DropSwordsFromSky(15));
    }

    IEnumerator DropSwordsFromSky(int counter)
    {
        float x_offset = Random.Range(-1, 1); // x값 부여
        float z_offset = Random.Range(-1, 1); // y값 부여
        GameObject earth = Instantiate(magicSwordFromSky, 
            new Vector3(player.transform.position.x + x_offset, player.transform.position.y + 6, 
            player.transform.position.z + z_offset), Quaternion.identity);
        yield return new WaitForSeconds(0.25f);
        if (counter > 0) // 검의 갯수 생성 제한
            StartCoroutine(DropSwordsFromSky(counter - 1));
    }

    public void CastAura()
    {
        if (!IsBossTakingDamage())
        { // 보스가 공격받고 있지 않다면
            Vector3 spawnPos = model.transform.position;
            spawnPos.y = 0.02f;
            GameObject aura = Instantiate(auraMagic, spawnPos, Quaternion.identity);
            aura.transform.eulerAngles = new Vector3(-90, 0, 0);
            aura.transform.position += new Vector3(0, 0.2f, 0);
        }
    }

    public void FireSpell() // 파이어볼 생성
    {
        if (!IsBossTakingDamage())
        {
            Vector3 relativePos = player.position - spellPosition.position;
            Instantiate(spell, spellPosition.position, Quaternion.LookRotation(relativePos, Vector3.up));
        }
    }

    public void Impact()
    {
        GameObject impactObj = Instantiate(impactPrefab, impactPosition.position, Quaternion.identity);
        Destroy(impactObj, 1.5f);
    }

    public void LightGreatSwordUp()
    {
        greatSword.gameObject.GetComponent<GreatSwordScript>().EnableGreatSwordFire(); // ativa o fogo da GreatSword

        Vector3 size = new Vector3(0.00075f, 0.0004f, 0.018f);
        Vector3 center = new Vector3(0f, 0f, 0.009f);
        SetGreatSwordSize(size, center);
        greatSword.gameObject.GetComponent<GreatSwordScript>().customSize += new Vector3(0, 0, 0.012f);
    }
    private void SetGreatSwordSize(Vector3 size, Vector3 center) // altera o tamanho da hitbox da GreatSword
    {
        greatSword.gameObject.GetComponent<BoxCollider>().size = size;
        greatSword.gameObject.GetComponent<BoxCollider>().center = center;
    }

    private void MagicFarSword()
    {
        GameObject obj = Instantiate(magicFarSword, greatSword.transform.position, Quaternion.identity);
        Destroy(obj, 4.5f);
    }

    #endregion

    #region 발차기 공격

    public void TurnKickColliderOn()
    {
        leftFootCollider.enabled = true;
        leftFootCollider.GetComponent<DamageDealer>().damageOn = true;
    }

    public void TurnKickColliderOff()
    {
        leftFootCollider.enabled = false;
        leftFootCollider.GetComponent<DamageDealer>().damageOn = false;
    }

    #endregion

    private void SetNotAttackingFalse()
    {
        anim.SetBool("NotAttacking", false);
    }

    private void SetNotAttackingTrue()
    {
        anim.SetBool("NotAttacking", true);
    }

    private void SetCanRotateTrue()
    {
        anim.SetBool("CanRotate", true);
    }

    private void SetCanRotateFalse()
    {
        anim.SetBool("CanRotate", false);
    }

}
