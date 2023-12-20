using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class Boss_AI : MonoBehaviour
{
    private Transform target;
    public Transform defaultPosition;
    public float movespeed = 400f;
    public float nextWaypointDistance = 2f;
    public float pathUpdateInterval = 0.9f;
    public Transform upperBound;
    public Transform lowerBound;
    public float maxPlayerDist = 5f;
    Path path;
    int currWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    private bool attacking = false;
    private bool isMoving = false;
    public  bool facingRight = false; 
    private float currTime = 0f;
    public float attack_cd = 5f;
    private boss_health bossHealth;
    private GameObject playerObject;

    public float riseSpeed = 2f;
    // Start is called before the first frame update
    
    
    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        target = playerObject.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<boss_health>();
        bossHealth.slider.gameObject.SetActive(false);
        InvokeRepeating("UpdatePath", 0f, pathUpdateInterval);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && IsTargetWithinGrid(target.position))
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);     
        }
        else if (seeker.IsDone() && !IsTargetWithinGrid(target.position))
        {
            seeker.StartPath(rb.position, defaultPosition.position, OnPathComplete);     
        }
        
    }

    bool IsTargetWithinGrid(Vector3 position) // checks if the player is in the given range
    {        
        float minX = lowerBound.position.x;
        float maxX = upperBound.position.x;
        //if (player_controller.invis) return false;
        return target.position.x >= minX && target.position.x <= maxX;
    }

    
    void OnPathComplete(Path p) // AI stuff to calculate path
    {
        if (!p.error)
        {
            path = p;
            currWaypoint = 0;
            
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
          
        if (path == null) return;

        
        if (currTime < attack_cd) currTime += Time.deltaTime;

        if (bossHealth.death) 
        {
            rb.velocity = new Vector2(0f,0f);
            return;
        }

        if (Mathf.Abs(rb.velocity.x) > 0f)
        {
            isMoving = true;
        }

        if (Mathf.Abs(rb.velocity.x) <= 0f)
        {
            isMoving = false;
        }

        if (reachedEndofPath && IsTargetWithinGrid(target.position)) 
        {   // if the enemy has reached the target and it within the range of the bounds
                
            if (!attacking && currTime >= attack_cd)
            {
                StartCoroutine(Attack());
                currTime = 0f;         
            }
        }

        if (currWaypoint >= path.vectorPath.Count)
        {
            
            reachedEndofPath = true;
            return;
        }

        else
        {
            reachedEndofPath = false;
        }

        // calculates the direction from enemy to the target
        Vector2 direction = ((Vector2)path.vectorPath[currWaypoint] - rb.position).normalized;
    
        float distance = Vector2.Distance(rb.position, path.vectorPath[currWaypoint]);
        // calculates the distance from enemy to the target
        
        if (direction.x > 0f && !facingRight || direction.x < 0f && facingRight) 
            Flip(); // handles the direction the enemy is facing

        
        /*float distance_player = transform.position.x - playerObject.transform.position.x;
        if (Mathf.Abs(distance_player) > maxPlayerDist && currTime >= attack_cd && !attacking){
            StartCoroutine(Attack());
        }*/

        if (!attacking) // handles enemy movement
        {  
            Vector2 force = direction * movespeed * Time.deltaTime;
            rb.velocity = new Vector2(movespeed*direction.x*Time.deltaTime, rb.velocity.y);
            
        }

        if (distance < nextWaypointDistance)
        {       
            currWaypoint++;
        }
        
        if (IsTargetWithinGrid(target.position)){
            bossHealth.slider.gameObject.SetActive(true);
            
        }
        else{
            bossHealth.slider.gameObject.SetActive(false);
            bossHealth.currHealth = bossHealth.maxHealth;
        }
        
    }

    void Flip() // flips the direction of the health bar and the sprite
    {
        transform.Rotate(0f,180f,0f);
        facingRight = !facingRight;
    }

    IEnumerator Attack(){
        float dir;
        currTime = 0f;
        attacking = true;
        float ogScale = rb.gravityScale;
        rb.gravityScale = 0;
        if (facingRight) dir = 1;
        else dir = -1; 
        rb.AddForce(new Vector2(30000f*dir, 15000f));
        yield return new WaitForSeconds(0f);
        rb.gravityScale = ogScale;
        attacking = false;
    }

    

}
