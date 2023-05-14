using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Temporary_Character_Movement : MonoBehaviour
{

    private float h_input;
    private float v_input;
    private float speed = 10.0f;
    private float jumpPower = 8f;
    private bool lockInPlace = false;
    
    [Flags]
    private enum Directions
    {
        Right = 1, 
        Left = 2,
        Up = 4, 
        Down = 8
    }

    private Directions current_Dir = Directions.Right;
    private float current_Angle = 0f;
    private Vector3 groundCheckOffset;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        groundCheckOffset = groundCheck.localPosition;
    }

    // Update is called once per frame
    void Update() 
    {
        h_input = Input.GetAxis("Horizontal");
        v_input = Input.GetAxis("Vertical");

        
        lockInPlace = isLock();
        Direction();

        if (lockInPlace) { return; }

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) // For duration of jumps. Higher jumps with a longer tap of the jump button
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


    }

    void FixedUpdate()
    {   
        if (lockInPlace)
        {
            rb.velocity = new Vector2(0f, 0f);
            return;
        }
        rb.velocity = new Vector2(h_input * speed, rb.velocity.y); // Time.deltaTime not needed when dealing with forces. 
    }
    
    private bool isLock()
    {   
        if (!isGrounded()) {  return false; }
        if (Input.GetKey(KeyCode.LeftControl)) { return true; } return false;
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer); // Creates a small invisible circle (radius of 0.2f) at the player's feet, and checks if it collides with groundLayer, which are all objects/tiles that have the 'ground' layer set
    }

    private void Direction() // Vertical input preceds horizontal input in terms of direction
    {
        Directions dir;
        if (lockInPlace)
        {
            dir = verticalInputDir() | horizontalInputDir(); // Combining the two directions
        } else 
        {
            dir = verticalInputDir() != 0 ? verticalInputDir() : horizontalInputDir(); 
        }

        if (dir !=0)
        {
            current_Dir = dir;
            RotateWeapon();
        }
        
    }


    private Directions verticalInputDir()
    {
        if (v_input > 0)
        {
            return Directions.Up;
        } else if (v_input < 0)
        {
            return Directions.Down;
        }
        return 0;
    }

    private Directions horizontalInputDir()
    {
        if (h_input > 0)
        {
            return Directions.Right;
        } else if (h_input < 0)
        {
            return Directions.Left;
        }
        return 0;
    }

    private void RotateWeapon()
    {

       
       switch (current_Dir)
        {
            case Directions.Right:
                current_Angle = 0f;
                break;
            case Directions.Up:
                current_Angle = 90f;
                break;
            case Directions.Left:
                current_Angle = 180f;
                break;
            case Directions.Down:
                current_Angle = 270f;
                break;
            case Directions.Right | Directions.Up:
                current_Angle = 45f;
                break;
            case Directions.Left | Directions.Up:
                current_Angle = 135f;
                break;
            case Directions.Left | Directions.Down:
                current_Angle = 225f;
                break;
            case Directions.Right | Directions.Down:
                current_Angle = 315f;
                break;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, current_Angle);
        groundCheck.position = transform.position + groundCheckOffset;
    }
}  
