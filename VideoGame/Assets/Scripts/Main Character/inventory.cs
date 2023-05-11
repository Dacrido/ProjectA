using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    private bool inventory_slots = true;
    private int inventory_length;

    public Transform inventoryLocation;
    
    private GameObject weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        
        inventory_length = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void pickUp(Collision2D col)
    {
        /*for (int i = 0; i < inventory_length; i++)
        {
            if (inventory_slots[i] == false){
                inventory_slots[i] = true;
                break;
            }
        }*/
        if (col.gameObject.tag == "Weapon")
        {
            weapon = col.gameObject;
            Collider2D otherCollider = col.collider;
            otherCollider.isTrigger = true;
                    
            weapon.transform.position = inventoryLocation.transform.position;
            weapon.transform.Rotate(0f,0f,transform.rotation.eulerAngles.y);
                    
            weapon.transform.SetParent(inventoryLocation.transform);
            inventory_slots = false;
        }
    }

    void dropOff()
    {
        Transform weapon = transform.GetChild(0);
        weapon.transform.position = inventoryLocation.transform.position;
        
        weapon.transform.SetParent(null);
        
    }

    void OnCollisionStay2D(Collision2D col) {
        
        if (col == null) return;
        
        if (Input.GetKey(KeyCode.E) && inventory_slots == true)
        {
            pickUp(col);          
        }
    
    }


}
