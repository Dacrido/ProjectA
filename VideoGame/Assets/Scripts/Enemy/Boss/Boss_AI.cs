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
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        target = playerObject.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<boss_health>();
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

        
        float distance_player = transform.position.x - playerObject.transform.position.x;
        if (Mathf.Abs(distance_player) > maxPlayerDist && currTime >= attack_cd){
            StartCoroutine(Attack());
        }


        if (!attacking) // handles enemy movement
        {  
            Vector2 force = direction * movespeed * Time.deltaTime;
            rb.velocity = new Vector2(movespeed*direction.x*Time.deltaTime, rb.velocity.y);
            
        }

        if (distance < nextWaypointDistance)
        {       
            currWaypoint++;
        }
        
        
    }

    void Flip() // flips the direction of the health bar and the sprite
    {
        transform.Rotate(0f,180f,0f);
        facingRight = !facingRight;
    }

    IEnumerator Attack(){
        Debug.Log("Attack!");
        currTime = 0f;
        attacking = true;
        
        yield return new WaitForSeconds(2f);
        attacking = false;

    }

}
