using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearEnemyTest : MonoBehaviour
{
    PlayerState playerState;
    Shooter shooter;
    PlayerAttack playerAttack;
    ChangeForm changeForm;

    [SerializeField]
    Collider[] enemys;              // ���ʹ� ���� �ݶ��̴�
    LayerMask enemyLayer;           // ���ʹ� ���� ���̾�
    public GameObject nearEnemy;    // ���� �����̿� �ִ� ���ʹ�
    float shortDist;                // ���� ����� �Ÿ� ���ϴ� ����

    float autoTargetRange = 0f;
    [SerializeField]
    float foxRange = 15f;
    [SerializeField]
    float tigerRange = 1.5f;
    [SerializeField]
    float eagleRange = 5f;

    void Awake()
    {
        playerState = GetComponent<PlayerState>();
        shooter = GetComponent<Shooter>();
        playerAttack = GetComponent<PlayerAttack>();
        changeForm = GetComponent<ChangeForm>();
        enemyLayer = LayerMask.NameToLayer("ENEMY");
    }

    private void OnEnable()
    {
        StartCoroutine(FindNearObject());
    }
    private void Update()
    {
        switch (changeForm.curForm)
        {
            case ChangeForm.FormType.FOX:
                autoTargetRange = foxRange;
                break;
            case ChangeForm.FormType.TIGER:
                autoTargetRange = tigerRange;
                break;
            case ChangeForm.FormType.EAGLE:
                autoTargetRange = eagleRange;
                break;
        }
    }

    IEnumerator FindNearObject()
    {
        int layerMask = (1 << enemyLayer);
        //0.3�ʸ��� 
        while (playerState.state != PlayerState.State.DIE)
        {
            yield return new WaitForSeconds(0.3f);
            // 0.3�ʸ��� 10���� �ȿ� �ִ� ���̾ ���ʹ��� �ݶ��̴����� ã�´�
            enemys = Physics.OverlapSphere(transform.position, autoTargetRange, layerMask);

            // �����ȿ� ���� �ִٸ�
            if (enemys.Length != 0)
            {
                shortDist = Vector3.Distance(transform.position, enemys[0].transform.position);
                nearEnemy = enemys[0].gameObject;   // ù��° ������Ʈ�� ����
                shooter.m_target = nearEnemy;
                playerAttack.target = nearEnemy;
                //nearEnemy.GetComponent<MeshRenderer>().material.color = Color.red;

                foreach (var enemy in enemys)
                {
                    if (enemy.gameObject == nearEnemy)
                        continue;
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distance < shortDist)
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
                // ��Ÿ� �ȿ� ���� ���� ���
                nearEnemy = null;
                shooter.m_target = null;
                playerAttack.target = null;
            }
        }
    }
}
