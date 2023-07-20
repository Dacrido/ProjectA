using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed, dirX, dirY;

    public bool ClimbingAllowed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (ClimbingAllowed)
        {
            dirY = Input.GetAxisRaw("Vertical") * moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (ClimbingAllowed)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(dirX, dirY);
        }
        else
        {
            rb.isKinematic = false;
            rb.velocity = new Vector2(dirX, rb.velocity);
        }
    }

}
