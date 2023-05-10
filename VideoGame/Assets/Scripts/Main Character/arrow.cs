using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 20f;
    public float maxDistance = 30f;


    public Rigidbody2D rb;
    public GameObject impact;
    public GameObject player;
    


    private controls other;


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

    
    void Damage()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Enemy"){
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    
}
