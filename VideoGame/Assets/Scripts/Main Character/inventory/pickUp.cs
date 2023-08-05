using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{
    private itemInventory inventory;
    public GameObject itemButton;
    private bool disablePickup = false;
    private float timeElapsed;
    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<itemInventory>();
    }

    public void Update()
    {
        if (timeElapsed < 0.3f){
            timeElapsed += Time.deltaTime;
            disablePickup = true;
        }

        else disablePickup = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.CompareTag("Player")){
            
            
            for (int i = 0; i < inventory.slots.Length; i++){
                if (inventory.isFull[i] == false && !disablePickup){
                        inventory.isFull[i] = true;
                        timeElapsed = 0f;
                        disablePickup = true;
                        Instantiate(itemButton, inventory.slots[i].transform, false);
                        Destroy(gameObject);
                        break;
                        
                    }
                }
            

        }
    }

}
