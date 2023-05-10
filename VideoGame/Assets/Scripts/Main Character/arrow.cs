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

    }

    
    void Damage()
    {
        
    }

    
}
