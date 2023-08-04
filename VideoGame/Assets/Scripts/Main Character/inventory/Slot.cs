using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private GameObject player;
    private shoot shootScript;
    private itemInventory inventory;
    public int i;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<itemInventory>();
        shootScript = player.GetComponent<shoot>();
    
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem(){
        foreach (Transform child in transform)
        {
            child.GetComponent<Spawn>().SpawnDroppedItem();
            if (child.gameObject.tag == "Arrow") shootScript.equipped_arrow = shootScript.default_arrow;

            GameObject.Destroy(child.gameObject);
        }
    }
}
