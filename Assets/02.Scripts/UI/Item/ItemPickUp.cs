using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
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
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);
        }

    }
}
