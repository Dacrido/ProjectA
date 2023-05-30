using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPotion : MonoBehaviour
{
    player_health health;
    private float rotationSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnCollisionStay2D(Collision2D col) { // check for any colision
        
        if (col == null) return; 
        
        if (col.gameObject.tag == "Player") // if an inventory slot is free and E is pressed
        {
            health = col.gameObject.GetComponent<player_health>();
            health.Heal(1);
            Destroy(gameObject);
        }

        
        
    
    }
}
