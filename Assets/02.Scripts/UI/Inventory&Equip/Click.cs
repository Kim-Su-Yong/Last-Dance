using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour, IPointerClickHandler
{
    UIManager uIManager;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        uIManager.selectedItem = this.gameObject;
        uIManager.isUse = true;
    }


    void Update()
    {
        
    }
}
