using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenOptions : MonoBehaviour
{
    public GameObject directionalLight;
    public Transform dropDown;

    public static ScreenOptions instance;
    void Update()
    {
        if (dropDown.GetComponent<Dropdown>().value == 0)
        {
            directionalLight.GetComponent<Light>().shadows = LightShadows.Soft;
        }
        if (dropDown.GetComponent<Dropdown>().value == 1)
        {
            directionalLight.GetComponent<Light>().shadows = LightShadows.Hard;
        }
        if (dropDown.GetComponent<Dropdown>().value == 2)
        {
            directionalLight.GetComponent<Light>().shadows = LightShadows.None;
        }
    }
}
