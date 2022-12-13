using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearEnemyTest : MonoBehaviour
{
    PlayerState playerState;
    Shooter shooter;
    PlayerAttack playerAttack;
    [SerializeField]
    Collider[] enemys;
    LayerMask enemyLayer;
    [SerializeField]
    public GameObject nearEnemy;   // 가장 가까이에 있는 에너미
    float shortDist;

    // Start is called before the first frame update
    void Awake()
    {
        playerState = GetComponent<PlayerState>();
        shooter = GetComponent<Shooter>();
        playerAttack = GetComponent<PlayerAttack>();
        enemyLayer = LayerMask.NameToLayer("ENEMY");
    }

    private void OnEnable()
    {
        StartCoroutine(FindNearObject());
    }

    IEnumerator FindNearObject()
    {
        int layerMask = (1 << enemyLayer);
        //0.3초마다 
        while (playerState.state != PlayerState.State.DIE)
        {
            yield return new WaitForSeconds(0.3f);
            // 0.3초마다 10범위 안에 있는 레이어가 에너미인 콜라이더들을 찾는다
            enemys = Physics.OverlapSphere(transform.position, 10f, layerMask);
            
            // 범위안에 적이 있다면
            if (enemys.Length != 0)
            {
                shortDist = Vector3.Distance(transform.position, enemys[0].transform.position);
                nearEnemy = enemys[0].gameObject;   // 첫번째 오브젝트를 기준
                shooter.m_target = nearEnemy;
                playerAttack.target = nearEnemy;
                //nearEnemy.GetComponent<MeshRenderer>().material.color = Color.red;

                foreach (var enemy in enemys)
                {
                    if (enemy.gameObject == nearEnemy)
                        continue;
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if(distance < shortDist)
                    {
                        nearEnemy = enemy.gameObject;
                        shortDist = distance;
                        shooter.m_target = nearEnemy;
                        playerAttack.target = nearEnemy;
                    }
                }
            }
            else
            {
                shooter.m_target = null;
                playerAttack.target = null;
            }
        }
    }
}
