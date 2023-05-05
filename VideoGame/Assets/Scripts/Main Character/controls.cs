using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{
    //movement
    private float horizontal; 
    private float moveSpeed = 5.5f;
    private float jumpForce = 6.6f;
    private bool isFacingRight = true;
    private bool canDoubleJump = true;



    //player physics and structure
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Transform groundCheck; //for checking the ground
    [SerializeField] private LayerMask groundLayer; // ground layer



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if left/right input is pressed
        horizontal = Input.GetAxisRaw("Horizontal");//horizontal input updated every frame
        
        //JUMP
        if (Input.GetButtonDown("Jump") && IsGround()) // if jump is pressed and the player is on the platform
        {       
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // accelerate the y velocity, i.e jump
            canDoubleJump = true;
           
        }
        
        // DOUBLE JUMP
        else if (Input.GetButtonDown("Jump") && canDoubleJump && !IsGround()) // if jump is pressed and the player is not on the platform
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce+0.5f); // // accelerate the y velocity, i.e jump
            canDoubleJump = false;
        
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*1f);        
        }

        Flip();
    
    }


    void FixedUpdate()
    {
        // movement left/right
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.33f, groundLayer);
        
    }

}
