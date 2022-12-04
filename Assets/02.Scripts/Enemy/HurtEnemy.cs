using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    public GameObject prefabs_floating_text;
    public GameObject parent;

    public string atkSound;

    private PlayerStat thePlayerStat;
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "ENEMY")
        {
            int dmg = col.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            SoundManager.instance.Play(atkSound);

            Vector3 vector = col.transform.position;
            vector.y += 60;

            GameObject clone = Instantiate(prefabs_floating_text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.white;
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);
        }
    }
}
