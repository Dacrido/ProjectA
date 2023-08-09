using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPotion : MonoBehaviour
{
    player_health health;
    
    // Start is called before the first frame update
    void Start()
    {
        health =  GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal()
    { // check for any colision
          
        health.Heal(1);
        Debug.Log("healed");
        Destroy(this.gameObject);
    }
}
