using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public float maxDistance = 30f;

    public float maxHeight;
    public Rigidbody2D rb;
    public bool impact;
    public GameObject player;
    
    public bool big_arrow = false; 

    private controls other;
    private EnemyHealth health;

    private int ammo;

    private float time_counter;
    private int direction;

    private float x_velocity;
    private float yposition;
    // Start is called before the first frame update
    void Awake(){
        player =  GameObject.FindGameObjectWithTag("Player");

        other = player.GetComponent<controls>(); 

    }
    
    void Start()
    {

        if (other.isFacingRight) direction = 1;

        else if (!other.isFacingRight) direction = -1;

        maxHeight = transform.position.y;
    }

    private void FixedUpdate() {
        
        if (!impact) rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        
        if (impact) rb.velocity = new Vector2(0f,0f);

        if (time_counter < 2.5) time_counter += Time.deltaTime;

        else Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D otherObj)
    {
        if (otherObj.gameObject.tag == "Map"){
            Destroy(gameObject);
        }
        
        if (otherObj.gameObject.tag == "Enemy")
        {
            health = otherObj.gameObject.GetComponent<EnemyHealth>();         
            health.takeDamage(damage);
            impact = true;
            Destroy(gameObject);

        }

        

        

    }

    
}
