using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{

    [SerializeField] private Animator anim;
    
     //movement
    private float horizontal; 
    //smoother movement using acceleration/deceleration system
    private float acceleration_rate = 50f;
    private float acceleration_time = 0.15f;
    private float time_counter;  
    private float currentSpeed;  
    private float maxSpeed = 5.5f;
    private float deceleration_time;
    private float deceleration_rate = 27.5f;

    // 
    //jump
    private float jumpForce = 7.5f;
    [System.NonSerialized] public bool isFacingRight = true;
    private bool canDoubleJump = true;
    
    //improving jump
    private float cayoteTime = 0.15f;
    private float cayoteTimeCounter;
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;
    private float dashDown = -15f;

    // dash
    private bool canDash = true;
    public bool isDashing;
    private float dashPower = 18.5f;
    private float dashTime = 0.2f;
    private float dashCooldown = 0.6f;


    

    private bool aim_mode = false;
    private float horizontal_input;
    private Vector2 direction;
    private float vertical_input;
    private float aimAngle;


    //player physics and structure
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Transform groundCheck; //for checking the ground
    [SerializeField] private LayerMask groundLayer; // ground layer

    



    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0f;
        time_counter = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if left/right input is pressed
        horizontal = Input.GetAxisRaw("Horizontal");//horizontal input updated every frame
        
        // cant do any other action while dashing
        if (isDashing) return; 


        // player can jump after a short amount of time after they have left the platform
        if (IsGround()) cayoteTimeCounter = cayoteTime;
        
        else cayoteTimeCounter -= Time.deltaTime;
        

        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBufferTime; 
        
        else jumpBufferCounter -= Time.deltaTime; 
        

        //DASH
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) StartCoroutine(Dash());
 

        

        //JUMP
        if (jumpBufferCounter > 0f && cayoteTimeCounter > 0f) // if it is within the cayote time and jump buffer time
        {       
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // accelerate the y velocity, i.e jump
            canDoubleJump = true;
            jumpBufferCounter = 0f;
           
        }
        // DOUBLE JUMP
        else if (Input.GetButtonDown("Jump") && canDoubleJump) // if jump is pressed and the player is not on the platform
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce-1f); // // accelerate the y velocity, i.e jump
            canDoubleJump = false;
        
        } 
        // When Jump is released
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*1f);
            cayoteTimeCounter = 0f;
        }
        
        if ((Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow) )) && !IsGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, dashDown);
            
        }
           
        
        
        if (horizontal < 0f && isFacingRight){
            Flip();
            isFacingRight = !isFacingRight;
        }

        if (horizontal > 0f && !isFacingRight){
            Flip();
            isFacingRight = !isFacingRight;
        }

        if (aim_mode)
        {                     
            vertical_input = Input.GetAxis("Vertical");  
                        
            
        }


        if (Input.GetKeyUp(KeyCode.Y)){
            aim_mode = !aim_mode;
            Debug.Log(aim_mode);
        }
    
    }


    void FixedUpdate()
    {
        if (isDashing) return;
        
        //rb.velocity = new Vector2(horizontal*maxSpeed, rb.velocity.y);

        //STILL UNDER TESTING
        //movement speed formula for smoother movement gameplay   
        
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
            rb.velocity = new Vector2(currentSpeed * Time.deltaTime, rb.velocity.y);
        }
          

        anim.SetFloat("speed", Mathf.Abs(horizontal));
               

    }

    private void Flip() 
    {
        // flipping the player sprite if they are moving in the opposite direction
        gameObject.transform.Rotate(0f,180f,0f);

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
        
        rb.velocity = new Vector2(horizontal*dashPower, 0f); // dash
        
        yield return new WaitForSeconds(dashTime); // dash for x seconds
        Physics2D.IgnoreLayerCollision(3, 7, false); // stop phasing
        rb.gravityScale = originalGravity; 
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown); //can't dash while cooldown
        canDash = true;
    }


    

}
