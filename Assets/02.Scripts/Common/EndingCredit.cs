using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    public GameObject endingCredit;
    public BossAI bossAI;
    public MouseHover mouseHover;

    void Start()
    {
        bossAI = GameObject.Find("Boss").GetComponent<BossAI>();
    }

    void Update()
    {
        if (!bossAI.isDie) return;
        else if (bossAI.isDie == true)
        {
            endingCredit.SetActive(true);
            Invoke("ToTitleScene", 15f);
        }
    }

    void ToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
