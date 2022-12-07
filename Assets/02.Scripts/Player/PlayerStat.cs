using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;
    public int[] needExp; //���� ����
    public int currentEXP;

    public int hp;
    public int currentHP;
    public int mp;
    public int currentMP;

    public int atk;
    public int def;

    public int recover_hp;
    public int recover_mp;

    public string dmgSound;

    [SerializeField]
    public GameObject prefabs_floating_text;
    [SerializeField]
    public GameObject parent;
    void Start()
    {
        instance = this;
        currentHP = hp;
    }
    public void Hit(int _enemyAtk)
    {
        int dmg;

        if (def >= _enemyAtk)
            dmg = 10;
        else
            dmg = _enemyAtk - def;

        currentHP -= dmg;

        if (currentHP <= 0)
            Debug.Log("ü�� 0�̸�, ���ӿ���");

        SoundManager.instance.Play(dmgSound);

        Vector3 vector = this.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_floating_text, vector, Quaternion.Euler(Vector3.zero));
        //clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        //clone.GetComponent<FloatingText>().text.color = Color.red;
        //clone.GetComponent<FloatingText>().text.fontSize = 25;
        //clone.transform.SetParent(parent.transform);
        //StopAllCoroutines();
        //StartCoroutine(HitCoroutine());
    }
    IEnumerator HitCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }
    void Update()
    {
        if (currentEXP >= needExp[character_Lv])
        {
            character_Lv++;
            hp += character_Lv * 2;
            mp += character_Lv + 2;

            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
        }
    }
}
