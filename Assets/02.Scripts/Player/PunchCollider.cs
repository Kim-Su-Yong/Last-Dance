using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollider : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    // 0.3초 후에 자동으로 비활성화
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
