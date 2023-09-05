using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mini_spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public boss_health health;
    public int spawn_count = 5;
    float spawn_timer = 0.3f;
    float deltaTime = 0.3f;
    public GameObject hopper_mini;
    public GameObject boss;
    private Transform last_pos;
    
    private void Awake(){
        boss = GameObject.FindGameObjectWithTag("Boss");
        
    }

    void Update()
    {
        
        
        if (deltaTime <= spawn_timer) deltaTime += Time.deltaTime;
        if (health.death && spawn_count > 0 && deltaTime >= 0.3f) 
        {
            Spawn();
            deltaTime = 0.0f;
            
        }
        
    }

    void Spawn(){     
        
        Instantiate(hopper_mini, gameObject.transform);
        spawn_count--;
    }
}
