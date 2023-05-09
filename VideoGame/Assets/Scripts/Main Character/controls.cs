using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{

    [SerializeField] private Animator anim;
    
     //movement
    [System.NonSerialized] public float horizontal; 
    //smoother movement using acceleration/deceleration system
    private float acceleration_rate = 36.66667f;
    private float acceleration_time = 0.15f;
    private float time_counter;  
    private float currentSpeed;  
    private float maxSpeed = 5.5f;
    private float deceleration_time;
    private float deceleration_rate = 27.5f;


    //jump
    private float jumpForce = 7.5f;
    [System.NonSerialized] public bool isFacingRight = true;
    private bool canDoubleJump = true;
    
    //improving jump
    private float cayoteTime = 0.15f;
    private float cayoteTimeCounter;
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    // dash
    private bool canDash = true;
    public bool isDashing;
    private float dashPower = 20f;
    private float dashTime = 0.2f;
    private float dashCooldown = 0.6f;

    

    //player physics and structure
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Transform groundCheck; //for checking the ground
    [SerializeField] private LayerMask groundLayer; // ground layer

    // shooting
    public Transform attackpoint;
    public Transform attackpoint2;
    public Transform attackpoint3;

    public GameObject arrowPrefab;
    public GameObject chargedArrow;
    
    
    private float nextAttackTime = 0f;
    private float attackRate = 1.5f;

    private float charge_time = 0.7f;
    private float time_counter_attack;


    private bool multiArrow = false;




    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0f;
        time_counter = 0f;
        time_counter_attack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //check if left/right input is pressed
        horizontal = Input.GetAxisRaw("Horizontal");//horizontal input updated every frame
        
        // cant do any other action while dashing
        if (isDashing)
        {   
            return;
        } 


        
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKey(KeyCode.F)){
                time_counter_attack += Time.deltaTime;
            }

            
            else if (Input.GetKeyUp(KeyCode.F)){
                if (time_counter_attack > charge_time) SpawnArrowHeavy();

                else SpawnArrowLight();
                

                nextAttackTime = Time.time + 1f/attackRate;
                time_counter_attack = 0;
            
            }
        }


        // player can jump after a short amount of time after they have left the platform
        if (IsGround())
        {
            cayoteTimeCounter = cayoteTime;
        } else
        {
            cayoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime; 
        } else
        {
            jumpBufferCounter -= Time.deltaTime; 
        }

        //DASH
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


        //JUMP
        if (jumpBufferCounter > 0f && cayoteTimeCounter > 0f) // if it is within the cayote time and jump buffer time
        {       
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // accelerate the y velocity, i.e jump
            canDoubleJump = true;
            jumpBufferCounter = 0f;
           
        }

        else if (Input.GetButtonDown("Jump") && canDoubleJump) // if jump is pressed and the player is not on the platform
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce-0.4f); // // accelerate the y velocity, i.e jump
            canDoubleJump = false;
        
        } 

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*1f);
            cayoteTimeCounter = 0f;
        }
        
        // DOUBLE JUMP
           
        if (Input.GetKeyUp(KeyCode.X))  multiArrow = !multiArrow;
        
        Flip();
    
    }


    void FixedUpdate()
    {
        if (isDashing)
        {        
            return;
        }  
        
        rb.velocity = new Vector2(horizontal*maxSpeed, rb.velocity.y);

        //STILL UNDER TESTING
        //movement speed formula for smoother movement gameplay   
        /*
        if (horizontal != 0){
            time_counter += Time.deltaTime;

            if (currentSpeed < maxSpeed) 
            {
                currentSpeed = acceleration_rate * time_counter;
            } else time_counter = 0;
            
            rb.velocity = new Vector2(currentSpeed * horizontal, rb.velocity.y);
        }

        else if (horizontal == 0) 
        {
            time_counter -= Time.deltaTime;
            
            if (currentSpeed > 0){
                currentSpeed = acceleration_rate * time_counter;
            } else {
                time_counter = 0;
                currentSpeed = 0;
            }
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
          */

        
               

    }

    private void Flip() // flipping the player sprite if they are moving in the opposite direction
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            
            attackpoint.transform.Rotate(0f,0f, 180f);
            attackpoint2.transform.Rotate(0f,0f, 180f);
            attackpoint3.transform.Rotate(0f,0f, 180f);

            
        }      
    }

    private bool IsGround() // checking if the player is standing on the platform
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.28f, groundLayer);
        
    }

    //dash
    private IEnumerator Dash()
    {  
        Physics2D.IgnoreLayerCollision(3, 7, true); // phase through enemies while dashing
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // we dont want gravity to affect while dashing
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f); // dash
        
        yield return new WaitForSeconds(dashTime); // dash for x seconds
        Physics2D.IgnoreLayerCollision(3, 7, false); // stop phasing
        rb.gravityScale = originalGravity; 
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown); //can't dash while cooldown
        canDash = true;
    }


    void SpawnArrowLight(){
        
        if (multiArrow){
            Instantiate(arrowPrefab, attackpoint.position, attackpoint.rotation);
            Instantiate(arrowPrefab, attackpoint2.position, attackpoint2.rotation);
            Instantiate(arrowPrefab, attackpoint3.position, attackpoint3.rotation);
            return;
        }
            
        Instantiate(arrowPrefab, attackpoint.position, attackpoint.rotation);

    }

    void SpawnArrowHeavy(){
        
        Instantiate(chargedArrow, attackpoint.position, attackpoint.rotation);

        
        
    }




}
