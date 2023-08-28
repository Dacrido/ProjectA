using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class Enemy_Chasing : MonoBehaviour, IMovementScript
{
    [SerializeField]
    private float _minTime;
    public float minTime
    {
        get { return _minTime; }
        set { _minTime = value; }
    }

    [SerializeField]
    private float _maxTime;
    public float maxTime
    {
        get { return _maxTime; }
        set { _maxTime = value; }
    }

    public bool canRepeat => throw new System.NotImplementedException();

    public bool needsLadder => throw new System.NotImplementedException();

    public bool isFlying => throw new System.NotImplementedException();

    public float distanceFromGround()
    {
        throw new System.NotImplementedException();
    }


    [Header("Pathfinding")]
    private Transform target;
    //public float activateDistance = 50f;
    private float pathUpdateSeconds = 0.3f;

    [Header("Physics")]
    //public float speed = 200f;
    private float nextWaypointDistance = 3f;
    //public float jumpNodeHeightRequirement = 0.8f;
    //public float jumpModifier = 0.3f;
    //public float jumpCheckOffset = 0.1f;

    private Vector2 collisionPosition;
    private bool collided;

    public float yFollowDistance;
    public float xFollowDistance;

    [Header("Custom Behavior")]
    //public bool followEnabled = true;
    //public bool jumpEnabled = true;
    //public bool directionLookEnabled = true;\

    private BoxCollider2D boxCollider;
    private Path path;
    private int currentWaypoint = 0;
    //RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;
    private Enemy_Behaviour General;

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        General = GetComponent<Enemy_Behaviour>();
        boxCollider = GetComponent<BoxCollider2D>();
        collided = false;

        
        
    }

    private void OnEnable()
    {
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        
    }

    private void OnDisable()
    {
        CancelInvoke("UpdatePath");
    }

    private void FixedUpdate()
    {
        CalculateDirection();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!enabled || !General.isFlying())
            return;  

        

        if (!collided && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            
            collided = true;
            collisionPosition = transform.position;

            Vector2 collisionNormal = collision.contacts[0].normal;
            Vector2 direction = new Vector2(-collisionNormal.x, -collisionNormal.y);

            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                General.setDirection(new Vector2(0, Mathf.Sign(General.getDirection().y)));
            } else
            {
                General.setDirection(new Vector2(Mathf.Sign(General.getDirection().x), 0));
            }

        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void CalculateDirection()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        //Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        //isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation

        if (General.isFlying())
        {
            if (collided)
            {

                if (Mathf.Abs(transform.position.x - collisionPosition.x) >= boxCollider.size.x / 2)
                    collided = false;
                else if (Mathf.Abs(transform.position.y - collisionPosition.y) >= boxCollider.size.y / 2)
                    collided = false;

            }
            else
            {
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                General.setDirection(direction);
            }
        } else
        {
            
            if (!General.isGrounded(General.getDistanceFromGround()))
            {
                if (transform.position.y - yFollowDistance > target.position.y)
                    General.stopMovement();
                else if (transform.position.x + xFollowDistance < target.position.x)
                    General.stopMovement();
                else if (transform.position.x - xFollowDistance > target.position.x)
                    General.stopMovement();
            } else
            {
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                General.setDirection(direction);
            }
            
            
        }
        

        

        //Vector2 force = direction * speed * Time.deltaTime;

        // Jump
        /*if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }*/

        // Movement
        //rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        /*if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }*/
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

}