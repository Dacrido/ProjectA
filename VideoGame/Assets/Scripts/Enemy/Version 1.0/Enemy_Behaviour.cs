using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;
using Debug = UnityEngine.Debug;
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
                
            }
            OnStateChanged();
        }
    }

    private HashSet<State> possible_States;


    // ************* SCRIPTS *************

    [SerializeField] private MonoBehaviour[] movementScripts;
    [SerializeField] private MonoBehaviour idle;
    [SerializeField] private bool idleOnGroundOnly;
    [SerializeField] private MonoBehaviour[] attackScripts; // contact damage is always applied, so does not take part in this array
    [SerializeField] private MonoBehaviour chaseScript;
    
    [SerializeField] private bool chaseAfterDamageTaken;
    private EnemyHealth enemy_health;
    private Enemy_Collision_Damage enemy_collision;

    // When chasing is implemented, it will need the use of the latest movement script to 'chase' the player with that movement. All the chasing script affects is direction

    [Tooltip("Current movement script")] private IMovementScript currentMovement;

    private float currentMinTime;
    private float currentMaxTime;
    private float chosenTime;
    [Tooltip("States: \n Default - timer to switch to idle state \n Idle - timer to switch to default state \n Chase - timer to switch back to default state")] private float timer;
    private float extraTime = 0.0f;

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
        Flying
    }

    private Type currentType;
    private Vector2 direction; // Either left/right/up/down. Up and down only possible in certain conditions such as using a ladder to chase the player upwards
    private Vector2 previousDirection;

    private BoxCollider2D boxCollider;


    // ************* LAYERS *************
    LayerMask groundLayer;
    LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        activeScript = null;
        disableScripts();
        getPossibleStates();

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");

        boxCollider = GetComponent<BoxCollider2D>();

        enemy_health = GetComponent<EnemyHealth>();
        enemy_health.onHealthChange.AddListener(enemyHealthChange);
        enemy_collision = GetComponent<Enemy_Collision_Damage>();
        enemy_collision.hitPlayer.AddListener(enemyHitPlayer);

        // Big problem with random is that all instances of random in all of the enemies are using the same seed. With the same seed, the same set of numbers occur
        // To fix this, we need a separate seed per script instance, and do this by setting the seed of random to a unique value based on the current time and the instance ID of the script
        random = new System.Random(DateTime.Now.Millisecond + GetInstanceID());
        direction = Vector2.left;
        previousDirection = direction;

        if (possible_States.Contains(State.Default))
        {
            currentState = State.Default;
        }
        else if (possible_States.Contains(State.Idle))
        {
            currentState = State.Idle;
            currentType = Type.Flying; // Idle at the start makes enemy stay in mid-air
            chooseDirection();
        }
        else {
            throw new Exception("Must have at least either movement or idle script attached");
        }
        //OnStateChanged(); // Calls to start the whole script activation process
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
        if (idle != null)
            idle.enabled = false;
        //foreach (MonoBehaviour script in attackScripts)
            //script.enabled = false;
        if (chaseScript != null)
            chaseScript.enabled = false;

    }

    void getPossibleStates()
    {
        possible_States = new HashSet<State>();
        if (movementScripts.Length != 0)
            possible_States.Add(State.Default);
        if (idle != null)
            possible_States.Add(State.Idle);
        if (chaseScript != null)
            possible_States.Add(State.Chase);
        if (attackScripts.Length != 0)
            possible_States.Add(State.Attack);
    }

    void getNextState(State nextState, State ifFirstNotPossible)
    {
        if (possible_States.Contains(nextState))
            currentState = nextState;
        else
            currentState = ifFirstNotPossible;
    }

    void OnStateChanged()
    {
        timer = 0f;
        switch (currentState)
        {
            case State.Default:
                if (direction != Vector2.zero)
                    previousDirection = direction;
                if (currentMovement != null)
                    (currentMovement as MonoBehaviour).enabled = false;
                chooseMovementScript();
                chooseDirection();
                activeScript = null;
                (currentMovement as MonoBehaviour).enabled = true;

                currentMinTime = currentMovement.minTime;
                currentMaxTime = currentMovement.maxTime;

                break;
            case State.Idle:
                if (direction != Vector2.zero)
                    previousDirection = direction; // previous direction is the direction the enemy faced before idling
                activeScript = idle;
                if (currentMovement != null)
                    (currentMovement as MonoBehaviour).enabled = false;
                currentMinTime = (idle as IMovementScript).minTime;
                currentMaxTime = (idle as IMovementScript).maxTime;
                break;
            case State.Chase:
                activeScript = chaseScript;
                if (currentMovement != null)
                    (currentMovement as MonoBehaviour).enabled = true;
                currentMinTime = (chaseScript as IMovementScript).minTime;
                currentMaxTime = (chaseScript as IMovementScript).maxTime;
                break;
            case State.Attack:
                //chooseAttackScript();
                if (currentMovement != null)
                    (currentMovement as MonoBehaviour).enabled = false;
                break;


        }

        float skewness = (currentMaxTime + currentMinTime) / 2;
        if (random.NextDouble() > 0.35d)
            currentMinTime = skewness;
        else
            currentMaxTime = skewness;
        chosenTime = (float) random.NextDouble() * (currentMaxTime - currentMinTime) + currentMinTime;
        chosenTime += extraTime;
        extraTime = 0f;
        

    }

    public bool isDefault()
    {
        return currentState == State.Default;
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
            switch (currentMovement.isFlying)
            {
                case true: currentType = Type.Flying; break;
                case false: currentType = Type.Ground; break;
            }
            return;
        }

        List<IMovementScript> possibilities = new List<IMovementScript>();

        // only canRepeat is a condition that can remove the possibility of a script being chosen. 
        foreach (IMovementScript script in movementScripts)
        {
            if (currentMovement == null)
            {
                possibilities.Add(script);
                continue;
            }

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
        switch (chosen.isFlying)
        {
            case true: currentType = Type.Flying; break;
            case false: currentType = Type.Ground; break;
        }

        currentMovement = chosen;
    }

    void chooseAttackScript()
    {

    }
    private void defaultBehaviour(float extra = 0.0f)
    {
        
        switch (currentType)
        {
            case Type.Ground:
                if (isGrounded() && timeTillChangeState())
                {
                    getNextState(State.Idle, State.Default);
                }
                    

                if (!isGrounded(extra) || isWalled())
                {
                    Flip();
                }
                break;

            case Type.Flying:
                if (idleOnGroundOnly)
                {
                    if (isGrounded(checkBothEnds: true) && timeTillChangeState())
                    {
                        getNextState(State.Idle, State.Default);
                    }
                        
                }
                else
                {
                    if (timeTillChangeState())
                    {
                        getNextState(State.Idle, State.Default);
                    }
                        
                }
                break;
        }

        if (possible_States.Contains(State.Chase) && seePlayer())
            currentState = State.Chase;       
        
    }

    private void idleBehaviour()
    {
        if (timeTillChangeState())
        {
            getNextState(State.Default, State.Idle);
        }
        if (possible_States.Contains(State.Chase) && seePlayer())
            currentState = State.Chase;
    }

    private void chaseBehaviour()
    {
        
        if (seePlayer()) // resets to 0 if sees the player
            timer = 0f;

        switch (currentType)
        {
            case Type.Ground:
                if (isGrounded(checkBothEnds: true) && timeTillChangeState())
                {
                    getNextState(State.Idle, State.Default);
                }
                break;
            case Type.Flying:
                if (timeTillChangeState())
                    getNextState(State.Idle, State.Default);
                break;
                
        }

        
        
    }


    private void attackBehaviour()
    {
        
    }

    private void enemyHealthChange(bool hit)
    {
        if (!chaseAfterDamageTaken)
            return;

        if (hit)
        {
            switch (currentState)
            {
                case State.Default:
                case State.Idle: currentState = State.Chase; break;
            }
        }
    }

    private void enemyHitPlayer()
    {
        if (!chaseAfterDamageTaken)
            return;

        switch (currentState)
        {
            case State.Default:
            case State.Idle: currentState = State.Chase; break;
        }

    }

    public void stopMovement() // Stops all movement (does not change to idle state, just stops movement)
    {
        extraTime = chosenTime - timer;
        getNextState(State.Idle, State.Default);
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
        
        switch (currentType)
        {
            case Type.Ground: // direction either left or right. Up/down only possible for those who can climb ladders, and only when next to a ladder. 
                direction = new Vector2(Random.Range(-1f, 1f), 0f);
                if (direction.x >= 0)
                {
                    direction = new Vector2(1, 0);
                } else
                    direction = new Vector2(-1, 0);

                break;
            case Type.Flying: // any direction
                direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; 
                break;
        }

        if (System.Math.Sign(previousDirection.x) *-1 == System.Math.Sign(direction.x))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

    }


    public Vector2 getDirection()
    {
        return direction;
    }

    public void setDirection(Vector2 direction) // only called from chase script
    {
        if (this.direction != Vector2.zero)
            previousDirection = this.direction;

        if (System.Math.Sign(previousDirection.x) * -1 == System.Math.Sign(direction.x))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        
        this.direction = direction; 
        

    }

    public float getDistanceFromGround()
    {
        return currentMovement.distanceFromGround();
    }

    public bool isFlying()
    {
        if (currentType == Type.Flying)
            return true;
        return false;
    }

    // extraRayDistance: extra distance added to the ray say if the enemy is jumping off the ground
    public bool isGrounded(float extraRayDistance = 0.0f, bool checkBothEnds = false) // Checks if the enemy is on the ground of not
    {
        // Getting position of downwards ray
        Vector2 ray_Position = transform.position;
        ray_Position.x += boxCollider.offset.x + direction.x * (boxCollider.size.x / 2); // Places the x position to the front of the enemy depending on direction
        ray_Position.y -= boxCollider.size.y / 2 - boxCollider.offset.y;

        // Direction and distance of downwards ray 
        Vector2 ray_Direction = Vector2.down;
        float ray_Distance = 0.2f + extraRayDistance;

        Vector2 box_Size = new Vector2(boxCollider.size.x, 0.01f);

        RaycastHit2D checkForGround = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, groundLayer);
        if (checkForGround.collider != null)
            return true;

        if (checkBothEnds)
        {
            ray_Position.x -= 2 * (boxCollider.offset.x + direction.x * (boxCollider.size.x / 2));

            checkForGround = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, groundLayer);
            if (checkForGround.collider != null)
                return true;
        }
        return false;

        
    }

    public bool isWalled(float extraRayDistance = 0.0f)
    {
        Vector2 ray_Position = transform.position;
        ray_Position.x += boxCollider.offset.x + direction.x * (boxCollider.size.x / 2);
        ray_Position.y += boxCollider.offset.y;

        Vector2 ray_Direction = direction;
        float ray_Distance = 0.05f + extraRayDistance;

        Vector2 box_Size = new Vector2(0.4f, boxCollider.size.y);

        RaycastHit2D checkForWall = Physics2D.BoxCast(ray_Position, box_Size, 0.0f, ray_Direction, ray_Distance, groundLayer); // Box cast so that an obstacle at any height compared to the enemy is detected
        if (checkForWall.collider != null)
            return true;

        return false;
    }

    public bool seePlayer() // sending 3 rays in a triangle like shape to detect the player
    {

        Vector2 ray_Position = transform.position;
        ray_Position.x += direction.x * (boxCollider.offset.x + boxCollider.size.x / 2);
        ray_Position.y += direction.y * (boxCollider.offset.y + boxCollider.size.y / 2);

        Vector2 ray_Direction = direction;
        float ray_Distance = 4.0f;

        float angleBetweenRays = 10.0f;

        int layerMask = LayerMask.GetMask("Player", "Ground"); // So that raycasts cant go through the ground

        RaycastHit2D hit = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        hit = Physics2D.Raycast(ray_Position, Quaternion.Euler(0, 0, angleBetweenRays) * ray_Direction, ray_Distance, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        hit = Physics2D.Raycast(ray_Position, Quaternion.Euler(0, 0, -angleBetweenRays) * ray_Direction, ray_Distance, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        
        return false;
    }

    public void Flip() // Must be updated to physically flip the enemy, as well as not flip the health bar
    {

        switch (currentType)
        {
            case Type.Ground:
                direction.x *= -1;
                break;
        }
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


}
