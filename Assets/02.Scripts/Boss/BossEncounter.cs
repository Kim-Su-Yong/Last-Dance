using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEncounter : MonoBehaviour
{
    public GameObject BossHpPanel;
    public GameObject BossHpBar;

    // πË∞Ê¿Ω
    AudioSource source;
    AudioClip mainBGM;
    AudioClip bossBGM;

    void Start()
    {
        BossHpBar = BossHpPanel.transform.GetChild(0).gameObject;
        source = Camera.main.GetComponent<AudioSource>();

        mainBGM = Resources.Load("Sound/MainTheme") as AudioClip;
        bossBGM = Resources.Load("Sound/BossTheme") as AudioClip;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log("hi");
            BossHpBar.SetActive(true);
            source.clip = bossBGM;
            source.Play();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Bye");
            BossHpBar.SetActive(false);
            source.clip = mainBGM;
            source.Play();
        }
    }
}
