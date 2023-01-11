using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    public GameObject endingCredit;
    public BossAI bossAI;
    public MouseHover mouseHover;
    GameObject player;

    void Start()
    {
        bossAI = GameObject.Find("Boss").GetComponent<BossAI>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!bossAI.isDie) return;
        else if (bossAI.isDie == true)
        {
            endingCredit.SetActive(true);
            Invoke("ToTitleScene", 14f);
            player.GetComponent<PlayerState>().state = PlayerState.State.TALK;
            UIManager.instance.isEnd = true;
        }
    }

    void ToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
