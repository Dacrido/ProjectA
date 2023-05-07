using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{

    [SerializeField] private Animator anim;



    //movement
    [System.NonSerialized] public float horizontal; 
    private float currentSpeed; 
    private float acceleration;
    private float maxSpeed = 5.3f;

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
    private float dashPower = 21.5f;
    private float dashTime = 0.2f;
    private float dashCooldown = 0.6f;

    

    //player physics and structure
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Transform groundCheck; //for checking the ground
    [SerializeField] private LayerMask groundLayer; // ground layer



    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //check if left/right input is pressed
        horizontal = Input.GetAxisRaw("Horizontal");//horizontal input updated every frame
        
        if (isDashing)
        {   
            return;
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce+0.8f); // // accelerate the y velocity, i.e jump
            canDoubleJump = false;
        
        } 

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y *1f);
            cayoteTimeCounter = 0f;
        }
        
        // DOUBLE JUMP
           

        Flip();
    
    }


    void FixedUpdate()
    {
        //movement speed formula for smoother gameplay

        // TESTING PURPOSES
        /*
        if (horizontal == 0 && currentSpeed > 0){
            while (currentSpeed > 0) currentSpeed -= 0.1f;
        }

        else if (horizontal != 0&& currentSpeed < maxSpeed){
            currentSpeed += 1f;
        }*/    
        //rb.velocity = new Vector2((horizontal) * currentSpeed, rb.velocity.y);
        
        if (isDashing)
        {        
            return;
        } 
        rb.velocity = new Vector2((horizontal) * maxSpeed, rb.velocity.y);

        

        

    }

    private void Flip() // flipping the player sprite if they are moving in the opposite direction
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            
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


}
