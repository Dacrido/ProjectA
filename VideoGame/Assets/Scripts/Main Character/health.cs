using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    [SerializeField] private float num_lives;
    public float current_health {get; private set;}
    // Start is called before the first frame update
    
    private float damage_cd;
    private float time_counter;

    private void Awake()
    {
        current_health = num_lives;
    }
    
    
    void Start()
    {
        num_lives = 1f;  
        damage_cd = 1f;
        time_counter = 0f;  
    }

    // Update is called once per frame
    void Update()
    {
        if (time_counter <= damage_cd)
        {
            time_counter += Time.deltaTime;
        }

        if (current_health <= 0f) 
        {
            Debug.Log("You Died!");
            Destroy(gameObject);

        }
        

    }


    void OnCollisionStay2D(Collision2D enemy) 
    {
        
        
        if (enemy.gameObject.tag == "Enemy" && time_counter >= damage_cd) 
        {
            TakeDamage(0.2f);
            Debug.Log("hit");         
            time_counter = 0f;
        }

    }

    public void TakeDamage(float damage)
    {
        current_health -= damage;       
    }



}
