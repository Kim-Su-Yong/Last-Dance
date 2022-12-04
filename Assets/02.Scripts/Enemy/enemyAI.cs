using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour
{
    public Image hpBar;
    public float hp = 100f;

    Transform tr;

    void Start() //에너미 데미지 테스트용임...... 플레이어 콜라이더 이슈 때문에 테스트는 못함
    {
        tr = GetComponent<Transform>();
        hpBar = GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<Image>();
        hpBar.color = Color.green;
    }

    
    void Update()
    {
        
    }
}
