using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;

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
    public float pathUpdateSeconds = 0.3f;

    [Header("Physics")]
    //public float speed = 200f;
    public float nextWaypointDistance = 3f;
    //public float jumpNodeHeightRequirement = 0.8f;
    //public float jumpModifier = 0.3f;
    //public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    //public bool followEnabled = true;
    //public bool jumpEnabled = true;
    //public bool directionLookEnabled = true;\

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
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        if (!General.isFlying())
        {
            
            /*if (General.isWalled())
            {
                General.stopMovement();
            } else
            
            if (!General.isGrounded())
            {
                float angle = Vector2.Angle(direction, Vector2.down);
                if (angle < 40f)
                {
                    General.continueMovement();
                } else
                {
                    General.stopMovement();
                }
                // Continue is movement direction is downwards at a steep incline (45 degrees down -> 90)
                // Else stop
            } else

            if (direction == Vector2.up)
            {
                General.stopMovement();
            }*/

            
            
            //direction = new Vector2(Mathf.Sign(direction.x), 0);
        } else
        {
            /*if (General.isGrounded())
                direction = new Vector2(Mathf.Sign(direction.x), 0);
            else if (General.isWalled())
                direction = new Vector2(0, Mathf.Sign(direction.y));*/

        }
           
        General.setDirection(direction);
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
