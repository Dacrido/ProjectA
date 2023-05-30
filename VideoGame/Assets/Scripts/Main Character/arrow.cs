using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public float maxDistance = 30f;


    public Rigidbody2D rb;
    public GameObject impact;
    public GameObject player;
    
    public bool big_arrow = false; 

    private controls other;
    private Health health;

    private int ammo;

    private float time_counter;
    
    // Start is called before the first frame update
    void Awake(){
        GameObject player = GameObject.Find("player_default");     
        other = player.GetComponent<controls>(); 
    }
    
    void Start()
    {

        if (other.isFacingRight) rb.velocity = new Vector2(speed, rb.velocity.y);

        else if (!other.isFacingRight) rb.velocity = new Vector2(-speed, rb.velocity.y);

    }

    void Update()
    {
        if (time_counter> 2.5f) Destroy(gameObject);
       
        time_counter += Time.deltaTime;
    }

    

    void OnCollisionEnter2D(Collision2D otherObj)
    {
        if (otherObj.gameObject.tag == "Map"){
            Destroy(gameObject);
        }
        
        if (otherObj.gameObject.tag == "Enemy")
        {
            health = otherObj.gameObject.GetComponent<Health>();
            
            if (big_arrow)
            {
                health.takeDamage(damage);
                return;
            }
            Destroy(gameObject);
            health.takeDamage(damage);

        }

        

        

    }

    
}
