using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Default_Hopping : MonoBehaviour
{

    private Rigidbody2D rb;
    private Enemy_General_Movement_Ground General;

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    private float startingHeight; // starting height before the jump

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        General = GetComponent<Enemy_General_Movement_Ground>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (General.isGrounded())
        {
            Jump();
        }

        General.DefaultReaction(transform.position.y - startingHeight);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(General.GetDirection() * speed, rb.velocity.y);
    }

    private void Jump()
    {   
        startingHeight = transform.position.y;
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }
}
