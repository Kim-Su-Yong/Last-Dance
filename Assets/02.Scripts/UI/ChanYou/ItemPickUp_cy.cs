using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp_cy : MonoBehaviour
{
    public int itemID;
    public int _count;
    //public string pickUpSound;

    public bool isPickUp = false;
    private readonly string playerTag = "Player";

    private void Update()
    {
        PickUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            isPickUp = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            isPickUp = false;
    }

    public void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPickUp)
        {
            Inventory_cy.instance.GetAnItem(itemID);
            Destroy(this.gameObject);
        }
        
    }
}
