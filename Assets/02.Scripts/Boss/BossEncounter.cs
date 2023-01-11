using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEncounter : MonoBehaviour
{
    public GameObject BossHpPanel;
    public GameObject BossHpBar;

    void Start()
    {
        BossHpBar = BossHpPanel.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log("hi");
            BossHpBar.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Bye");
            BossHpBar.SetActive(false);
        }
    }
}
