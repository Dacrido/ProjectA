using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;



/*
 * General script managing all movement, chase and attack scripts for all kinds of enemies. Enemies will have 3 states, default (movement), chase (the player) and attack (the player). 
 * This script will manage which script from all 3 states will be active at one time. In addition, there can be multiple of each type of scripts. For example, multiple attack and 
 * movement scripts. Based on various conditions, the one currently activated depends on a set of conditions. These conditions are varied based on the state, movement, chasing or attack, 
 * with various conditions that must be satisfied in order for it to be performed. 
 * 
 * TO DO:
 *          1. Manage movement scripts
 *          2. Manage chase scripts, as well as change between states
 *          3. Manage attack scripts (attack script should be considered finished when attack animation is over)
 * 
 */
public class Enemy_Behaviour : MonoBehaviour
{

    // ************* GENERAL *************

    System.Random random;


    // ************* STATES *************
    private enum State
    {
        Default, 
        Idle,
        Chase, 
        Attack
    }

    private State _currentState;
    private State currentState
    {
        get { return _currentState; }
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnStateChanged();
            }
        }
    }


    // ************* SCRIPTS *************
    [SerializeField] private MonoBehaviour[] movementScripts;
    [SerializeField] private MonoBehaviour idle;
    [SerializeField] private MonoBehaviour[] attackScripts; // contact damage is always applied, so does not take part in this array
    [SerializeField] private MonoBehaviour chaseScript;

    // When chasing is implemented, it will need the use of the latest movement script to 'chase' the player with that movement. All the chasing script affects is direction

    [Tooltip("Current movement script")] private IMovementScript currentMovement;

    private float currentMinTime;
    private float currentMaxTime;
    private float chosenTime;
    [Tooltip("States: \n Default - timer to switch to idle state \n Idle - timer to switch to default state \n Chase - timer to switch back to default state")] private float timer;

    private MonoBehaviour _activeScript;
    [Tooltip("Which non-movement script is currently active. Null otherwise")] private MonoBehaviour activeScript
    {
        get { return _activeScript;  }
        set {
            if (_activeScript != value)
            {
                if (_activeScript != null)
                    _activeScript.enabled = false;

                _activeScript = value;

                if (_activeScript != null)
                    _activeScript.enabled = true;                        
                
                
            }
        }
    }  
    


    // ************* STATS **************
    public enum Type
    {
        Ground,
        Flying, 
        Mix
    }

    public Type type;
    private Vector2 direction; // Either left/right/up/down. Up and down only possible in certain conditions such as using a ladder to chase the player upwards

    private BoxCollider2D boxCollider;


    // ************* LAYERS *************
    LayerMask groundLayer;
    LayerMask playerLayer;


    // Start is called before the first frame update
    void Start()
    {
        activeScript = null;
        disableScripts();

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");

        boxCollider = GetComponent<BoxCollider2D>();

        // Big problem with random is that all instances of random in all of the enemies are using the same seed. With the same seed, the same set of numbers occur
        // To fix this, we need a separate seed per script instance, and do this by setting the seed of random to a unique value based on the current time and the instance ID of the script
        random = new System.Random(DateTime.Now.Millisecond + GetInstanceID());




        currentState = State.Default;
        chooseDirection();
        chooseMovementScript(); // Everything starts in 'default' state
        OnStateChanged(); // Calls to start the whole script activation process
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch (currentState)
        {   // Default, Idle and Chase must always have seePlayer on, in order to chase the player
            case State.Default:
                defaultBehaviour(currentMovement.distanceFromGround());
                break;
            case State.Idle:
                idleBehaviour();
                break;
            case State.Chase:
                chaseBehaviour();
                break;
            case State.Attack:
                attackBehaviour();
                break;
        }
    }



    // ******************************************************************** SCRIPTS *********************************************************************
    void disableScripts()
    {
        foreach (MonoBehaviour script in movementScripts)
            script.enabled = false;
        idle.enabled = false;
        //foreach (MonoBehaviour script in attackScripts)
            //script.enabled = false;
        //chaseScript.enabled = false;

    }

    void OnStateChanged()
    {

        timer = 0f;

        switch (currentState)
        {
            case State.Default:

                (currentMovement as MonoBehaviour).enabled = false;
                chooseMovementScript();

                activeScript = null;
                (currentMovement as MonoBehaviour).enabled = true;

                currentMinTime = currentMovement.minTime;
                currentMaxTime = currentMovement.maxTime;

                break;
            case State.Idle:
                activeScript = idle;
                (currentMovement as MonoBehaviour).enabled = false;
                currentMinTime = (idle as IMovementScript).minTime;
                currentMaxTime = (idle as IMovementScript).maxTime;
                break;
            case State.Chase:                
                activeScript = chaseScript;
                (currentMovement as MonoBehaviour).enabled = true;
                //currentMinTime = minChaseTime;
                //currentMaxTime = maxChaseTime;
                break;
            case State.Attack:
                //chooseAttackScript();
                (currentMovement as MonoBehaviour).enabled = false;
                break;

        }

        float skewness = (currentMaxTime - currentMinTime) / 2;
        if (random.NextDouble() > 0.35d)
            currentMinTime = skewness;
        else
            currentMaxTime = skewness;
        chosenTime = (float) random.NextDouble() * (currentMaxTime - currentMinTime) + currentMinTime;
        print(chosenTime);
    }

    /*
     * Movement script conditions: 
     *            - Min time performed
     *            - Max time performed
     *            - can repeat (if using multiple scripts and want them to alternate between each other never repeating twice in a row --> false)
     *            - needs ladder --> when ladder system is made. Only for 'ground' enemies
     *            - is flying (if enemy can both walk and fly, extra code is needed to allow for transition between the two, like gravity)
     */
    void chooseMovementScript()
    {
        if (movementScripts.Length == 1)
        {
            currentMovement = (IMovementScript) movementScripts[0];
            return;
        }

        List<IMovementScript> possibilities = new List<IMovementScript>();

        // only canRepeat is a condition that can remove the possibility of a script being chosen. 
        foreach (IMovementScript script in movementScripts)
        {

            if (script.canRepeat)
            {
                possibilities.Add(script);

            } else if (!(script.GetType() == currentMovement.GetType()))
            {
                possibilities.Add(script);
            }
        }
        int randomIndex = Random.Range(0, possibilities.Count);
        IMovementScript chosen = possibilities[randomIndex];

        currentMovement = chosen;
    }

    void chooseAttackScript()
    {

    }
    private void defaultBehaviour(float extra = 0.0f)
    {
        if (isGrounded() && timeTillChangeState())
            currentState = State.Idle;

        if (!isGrounded(extra) || isWalled())
        {
            Flip();
        }
        
    }

    private void idleBehaviour()
    {
        if (timeTillChangeState())
            currentState = State.Default;
    }

    private void chaseBehaviour()
    {   

        if (seePlayer()) // resets to 0 if sees the player
            timer = 0f;

        if (timeTillChangeState())
            currentState = State.Default;
        
    }

    private void attackBehaviour()
    {
        
    }



    private bool timeTillChangeState()
    {   
        if (timer >= chosenTime)
        {
            return true;
        }
        return false;
    }

    // ************************************************************************************ STATS ****************************************************************************
    void chooseDirection()
    {   
        switch (type)
        {
            case Type.Ground: // direction either left or right. Up/down only possible for those who can climb ladders, and only when next to a ladder. 
                direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
                break;
            case Type.Flying: // any direction
                direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; 
                break;
            case Type.Mix: // must first determine whether in ground state or flying state via active movement                 
                break;
        }
    }


    public Vector2 getDirection()
    {
        return direction;
    }

    // extraRayDistance: extra distance added to the ray say if the enemy is jumping off the ground
    public bool isGrounded(float extraRayDistance = 0.0f) // Checks if the enemy is on the ground of not
    {
        // Getting position of downwards ray
        Vector2 ray_Position = transform.position;
        ray_Position.x += boxCollider.offset.x + direction.x * (boxCollider.size.x / 2); // Places the x position to the front of the enemy depending on direction
        ray_Position.y -= boxCollider.size.y / 2 - boxCollider.offset.y;

        // Direction and distance of downwards ray
        Vector2 ray_Direction = Vector2.down;
        float ray_Distance = 0.1f + extraRayDistance;

        RaycastHit2D checkForGround = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, groundLayer); // May have to change to a boxcast **********************************

        if (checkForGround.collider != null)
            return true;
        return false;
    }

    public bool isWalled()
    {
        Vector2 ray_Position = transform.position;
        ray_Position.x += boxCollider.offset.x + direction.x * (boxCollider.size.x / 2);
        //ray_Position.y += boxCollider.offset.y;

        Vector2 ray_Direction = direction * Vector2.right;
        float ray_Distance = 0.05f;

        Vector2 ray_Size = new Vector2(0.4f, boxCollider.size.y);

        RaycastHit2D checkForWall = Physics2D.BoxCast(ray_Position, ray_Size, 0.0f, ray_Direction, ray_Distance, groundLayer); // Box cast so that an obstacle at any height compared to the enemy is detected

        if (checkForWall.collider != null)
            return true;

        return false;
    }

    public bool seePlayer()
    {

        Vector2 ray_Position = transform.position;
        ray_Position.x += direction.x * (boxCollider.offset.x + boxCollider.size.x / 2);

        Vector2 ray_Direction = direction * Vector2.right;
        float ray_Distance = 4.0f;

        RaycastHit2D checkForPlayer = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, playerLayer);

        if (checkForPlayer.collider != null)
            return true;
        return false;
    }

    public void Flip() // Must be updated to physically flip the enemy, as well as not flip the health bar
    {
        direction *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


}
