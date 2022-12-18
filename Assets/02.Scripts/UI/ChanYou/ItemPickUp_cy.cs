using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp_cy : MonoBehaviour
{
    public int itemID;
    public int _count;
    //public string pickUpSound;

    public GameObject prefab_floating_text;
    

    private readonly string playerTag = "Player";

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag) && Input.GetKeyDown(KeyCode.K))
        {
            Inventory_cy.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);
        }

    }
}
