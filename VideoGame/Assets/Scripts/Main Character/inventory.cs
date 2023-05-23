using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    private bool inventory_slots = true; // to check if inventory is free
    private int inventory_length; // not implemented right now

    public Transform inventoryLocation; // get the inventory object
    
    private GameObject weapon; // get the weapon object
    
    // Start is called before the first frame update
    void Start()
    {    
        inventory_length = 2; // not functional
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void pickUp(Collision2D col) // pick up weapon 
    {
        
        if (col.gameObject.tag == "Weapon") // if the collision is a weapon type
        {
            weapon = col.gameObject; // store the weapon object in a variable
            Collider2D otherCollider = col.collider; // get the collider of that weapon
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>(); // get the rigidbody of the weapon
            rb.bodyType = RigidbodyType2D.Kinematic; // set the rigidbody to kinematic so it doesnt fall off the players hand
            otherCollider.isTrigger = true; // turn off collision because no two objects can occupy the exact same space
                    
            weapon.transform.position = inventoryLocation.transform.position; // turn the weapons position to the inventory location's position
            weapon.transform.eulerAngles = new Vector3(weapon.transform.eulerAngles.x,transform.eulerAngles.y,weapon.transform.eulerAngles.z); // set rotation to the players current location
                    //Mathf.Abs(transform.rotation.eulerAngles.y) // if i ever forget this
            weapon.transform.SetParent(inventoryLocation.transform); // setting the weapon as a child of the inventory so it moves with the player
            inventory_slots = false; // no more weapons can be picked up
        }
    }

    void dropOff(Collider2D weapon) // dropping the weapon
    {       
        weapon.isTrigger = false; // turn on collision again
        Rigidbody2D rb = weapon.gameObject.GetComponent<Rigidbody2D>(); // get the weapons rigid body
        rb.bodyType = RigidbodyType2D.Dynamic; // turn the rigidbody type to dynamic // basically turns on gravity
        // object still doesn't move in the x direction because movement in the X direction is frozen (check Inspector window)
        
        inventoryLocation.transform.DetachChildren(); // detaching it from the parent (inventory)   
    }

    void OnCollisionStay2D(Collision2D col) { // check for any colision
        
        if (col == null) return; 
        
        if (Input.GetKey(KeyCode.E) && inventory_slots == true) // if an inventory slot is free and E is pressed
        {
            pickUp(col);          
        }
    
    }

    void OnTriggerStay2D(Collider2D weapon){ // check for any trigger collisions
        
        if (Input.GetKey(KeyCode.G)) // if g is pressed
        {
            dropOff(weapon);
            inventory_slots = true;          
        }
    }


}
