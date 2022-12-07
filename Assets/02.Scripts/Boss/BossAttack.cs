using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BossAttack : MonoBehaviour
{
    [Header("Control")]
    public bool AI;
    public bool debug;

    [Header("References")]
    public Transform model;
    public Transform player;
    public BoxCollider leftFootCollider; 
    //public Transform spellPosition;
    //public Transform impactPosition; 
    private Animator playerAnim;
    //public DamageDealer greatSword;
    //public CameraShaker shaker;
    //public GameManagerScript gameManager;

    //[Header("Attacks")]
    //public GameObject earthShatterPrefab;
    //public GameObject magicSwordFromSky;
    //public GameObject spell;
    //public GameObject auraMagic;
    //public GameObject screamMagic;
    //public GameObject magicFarSword;
    //public GameObject impactPrefab;

    private Animator anim;

    //[Header("Debug")]
    //public GameObject brainIcon;
    //public Image bossAttackingDebug;
    //public Image bossMovingDebug;
    //public Text walkTimeDebug;
    //public Text distanceDebug;
    //public Text brainDebug;
    //public Text damageDebug;
    //public Text speedText;
    //public Color farColor;
    //public Color middleColor;
    //public Color nearColor;

    [Header("AI Manager")]
    public float nearValue;
    public float farValue;
    public float chillTime;
    private string action;
    private float lastActionTime;
    private float distance;
    private float chillDirection;
    private bool phase2;
    private bool canBeginAI;
    private int lastAttack;

    // SlowBossDown
    private bool slowDown;
    private string actionAfterSlowDown;


    private void Start()
    {
        anim = model.GetComponent<Animator>();
        //playerAnim = player.GetComponent<Animator>();

        Vector3 size = new Vector3(0.00075f, 0.0004f, 0.014f);
        Vector3 center = new Vector3(0f, 0f, 0.007f);
        //SetGreatSwordSize(size, center);
    }

    private void Update()
    {
        //speedText.text = anim.GetFloat("Vertical").ToString("0.0");

        //if (Input.GetKeyDown(KeyCode.Keypad0) && gameManager.master) AI = !AI;
        //if (Input.GetKeyDown(KeyCode.Keypad1) && gameManager.master) debug = true;
        //brainIcon.gameObject.SetActive(AI);

        //distance = Vector3.Distance(model.transform.position, player.transform.position);

        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        //if (debug)
        //    //DebugUI();

        if (distance < 20 && !anim.GetBool("Equipped"))
        {
            //anim.SetTrigger("DrawSword");
            StartCoroutine(StartAI());
        }

        if (!anim.GetBool("Equipped")) return;

        if (!canBeginAI) return;

        if (AI && !playerAnim.GetBool("Dead"))
        {
            AI_Manager();
        }
        //else if (gameManager.master)
        //{
        //    DebugAttack(); 
        //}
        else
        {
            anim.SetBool("GameEnd", true);
            anim.SetBool("CanRotate", false);
        }

        //greatSword.damageOn = anim.GetBool("Attacking");

        phase2 = anim.GetBool("Phase2");

    }

    IEnumerator StartAI()
    {
        yield return new WaitForSeconds(4);
        canBeginAI = true;
    }

    //private void DebugUI()
    //{
    //    speedText.transform.parent.gameObject.SetActive(true);
    //    distanceDebug.transform.parent.gameObject.SetActive(true);
    //    brainDebug.transform.parent.gameObject.SetActive(true);
    //    damageDebug.text = greatSword.damageAmount.ToString();
    //    bossAttackingDebug.gameObject.SetActive(anim.GetBool("Attacking"));
    //    bossMovingDebug.gameObject.SetActive(action == "Move");
    //    distanceDebug.text = distance.ToString("0.0"); // mostra a distancia no debug
    //    if (distance <= nearValue) distanceDebug.color = nearColor;
    //    else if (distance >= farValue) distanceDebug.color = farColor;
    //    else distanceDebug.color = middleColor;
    //}

    private void FarAttack()
    {
        //brainDebug.text = "Far Attack";
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);

        int rand = 0;
        do
        {
            if (!anim.GetBool("Phase2")) rand = Random.Range(0, 7);
            if (anim.GetBool("Phase2")) rand = Random.Range(0, 8);
        } while (rand == lastAttack);
        lastAttack = rand;

        if (anim.GetBool("Phase2") && Random.Range(0, 2) == 0)
        {
            anim.SetTrigger("Spell");
        }

        switch (rand)
        {
            case 0:
                anim.SetTrigger("Casting");
                break;
            case 1:
                anim.SetTrigger("Dash");
                break;
            case 2:
                anim.SetTrigger("DoubleDash");
                break;
            case 3:
                anim.SetTrigger("Spell");
                break;
            case 4:
                anim.SetTrigger("Scream");
                break;
            case 5:
                anim.SetTrigger("Fishing");
                break;
            case 6:
                anim.SetTrigger("SuperSpinner");
                break;
            default:
                break;
        }

        action = "Wait";
    }

    private void NearAttack()
    {
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);

        int rand = 0;
        do
        {
            if (!anim.GetBool("Phase2")) rand = Random.Range(0, 10);
            if (anim.GetBool("Phase2")) rand = Random.Range(0, 13);
        } while (rand == lastAttack);
        lastAttack = rand;

        switch (rand)
        {
            case 0:
                anim.SetTrigger("DoubleDash");
                //brainDebug.text = "Double Dash";
                break;
            case 1:
                anim.SetTrigger("Dash");
                //brainDebug.text = "Dash";
                break;
            case 2:
                anim.SetTrigger("SpinAttack");
                //brainDebug.text = "Spin Attack";
                break;
            case 3:
                anim.SetTrigger("Combo");
                //brainDebug.text = "Combo";
                break;
            case 4:
                anim.SetTrigger("Casting");
                //brainDebug.text = "Casting";
                break;
            case 5:
                anim.SetTrigger("Combo1");
                //brainDebug.text = "Combo1";
                break;
            case 6:
                anim.SetTrigger("AuraCast");
                //brainDebug.text = "Aura Cast";
                break;
            case 7:
                anim.SetTrigger("ForwardAttack");
                //brainDebug.text = "ForwardAttack";
                break;
            case 8:
                anim.SetTrigger("Scream");
                //brainDebug.text = "Scream";
                break;
            case 9:
                anim.SetTrigger("Impact");
                //brainDebug.text = "Impact";
                break;
            case 10:
                anim.SetTrigger("Strong");
                //brainDebug.text = "Strong";
                break;
            case 11:
                anim.SetTrigger("JumpAttack");
                //brainDebug.text = "Jump Attack";
                break;
            default:
                break;
        }

        action = "Wait";

    }

    private void SlowBossDown()
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
            //brainDebug.text = "SlowDown";
            anim.SetFloat("Vertical", Mathf.Lerp(anim.GetFloat("Vertical"), 0, 1 * Time.deltaTime));
        }
    }

    IEnumerator WaitAfterNearMove()
    {
        //brainDebug.text = "WaitRandomly";
        slowDown = false;
        action = "Wait";
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Horizontal", 0);
        float maxWaitTime = 6;
        float possibility = 2;
        if (anim.GetBool("Phase2"))
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
        CallNextMove();
    }

    private void MoveToPlayer()
    {
        //brainDebug.text = "Move";

        anim.SetFloat("Horizontal", 0);

        float speedValue = distance / 15;
        if (speedValue > 1) speedValue = 1;

        //walkTimeDebug.text = (Time.time - lastActionTime).ToString("0.0");

        if (slowDown)
        {
            SlowBossDown();
            return;
        }

        if (distance < nearValue)
        {
            //anim.SetFloat("Vertical", 0);
            //CallNextMove();

            actionAfterSlowDown = "CallNextMove";
            slowDown = true;
        }
        else if (Time.time - lastActionTime > chillTime)
        {
            //anim.SetFloat("Vertical", 0);
            //action = "FarAttack";

            actionAfterSlowDown = "FarAttack";
            slowDown = true;
        }
        else
        {
            anim.SetFloat("Vertical", speedValue);
        }
    }

    private void WaitForPlayer()
    {
        //brainDebug.text = "Chill";

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

    private bool IsBossTakingDamage() 
    {
        return !anim.GetCurrentAnimatorStateInfo(2).IsName("none");
    }

    #region Debug

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

    #region Magics
    //public void SpawnEarthShatter()
    //{
    //    Vector3 bossPos = model.transform.position;
    //    Vector3 bossDirection = model.transform.forward;
    //    Quaternion bossRotation = model.transform.rotation;
    //    float spawnDistance = 3;

    //    Vector3 spawnPos = bossPos + bossDirection * spawnDistance;
    //    GameObject earthShatter = Instantiate(earthShatterPrefab, spawnPos, Quaternion.identity);
    //    earthShatter.transform.rotation = bossRotation;
    //    Destroy(earthShatter, 4);

    //    //shaker.ShakeCamera(1.5f);
    //}

    //public void Scream()
    //{
    //    GameObject scream = Instantiate(screamMagic, model.transform.position, Quaternion.identity);
    //    scream.transform.eulerAngles = new Vector3(90, 0, 0);
    //}

    //public void SwordsFromSkyAttack()
    //{
    //    StartCoroutine(DropSwordsFromSky(15));
    //}

    //IEnumerator DropSwordsFromSky(int counter)
    //{
    //    float x_offset = Random.Range(-1, 1); 
    //    float z_offset = Random.Range(-1, 1); 
    //    GameObject earth = Instantiate(magicSwordFromSky, new Vector3(player.transform.position.x + x_offset, 
    //        player.transform.position.y + 6, player.transform.position.z + z_offset), Quaternion.identity);
    //    yield return new WaitForSeconds(0.25f);
    //    if (counter > 0)
    //        StartCoroutine(DropSwordsFromSky(counter - 1));
    //}

    //public void CastAura()
    //{
    //    if (!IsBossTakingDamage())
    //    { 
    //        Vector3 spawnPos = model.transform.position;
    //        spawnPos.y = 0.02f;
    //        GameObject aura = Instantiate(auraMagic, spawnPos, Quaternion.identity);
    //        aura.transform.eulerAngles = new Vector3(-90, 0, 0);
    //        aura.transform.position += new Vector3(0, 0.2f, 0);
    //    }
    //}

    //public void FireSpell()
    //{
    //    if (!IsBossTakingDamage())
    //    {
    //        Vector3 relativePos = player.position - spellPosition.position;
    //        Instantiate(spell, spellPosition.position, Quaternion.LookRotation(relativePos, Vector3.up));
    //    }
    //}

    //public void Impact()
    //{
    //    GameObject impactObj = Instantiate(impactPrefab, impactPosition.position, Quaternion.identity);
    //    Destroy(impactObj, 1.5f);
    //    //shaker.ShakeCamera(0.5f);
    //}

    //public void LightGreatSwordUp()
    //{
    //    greatSword.gameObject.GetComponent<GreatSwordScript>().EnableGreatSwordFire();

    //    Vector3 size = new Vector3(0.00075f, 0.0004f, 0.018f);
    //    Vector3 center = new Vector3(0f, 0f, 0.009f);
    //    SetGreatSwordSize(size, center);
    //    greatSword.gameObject.GetComponent<GreatSwordScript>().customSize += new Vector3(0, 0, 0.012f);
    //}
    //private void SetGreatSwordSize(Vector3 size, Vector3 center)
    //{
    //    greatSword.gameObject.GetComponent<BoxCollider>().size = size;
    //    greatSword.gameObject.GetComponent<BoxCollider>().center = center;
    //}

    //private void MagicFarSword()
    //{
    //    GameObject obj = Instantiate(magicFarSword, greatSword.transform.position, Quaternion.identity);
    //    Destroy(obj, 4.5f);
    //}

    #endregion

    #region Kick

    //public void TurnKickColliderOn()
    //{
    //    leftFootCollider.enabled = true;
    //    leftFootCollider.GetComponent<DamageDealer>().damageOn = true;
    //}

    //public void TurnKickColliderOff()
    //{
    //    leftFootCollider.enabled = false;
    //    leftFootCollider.GetComponent<DamageDealer>().damageOn = false;
    //}

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