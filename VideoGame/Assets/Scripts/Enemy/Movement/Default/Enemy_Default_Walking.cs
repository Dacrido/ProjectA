using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Default_Walking : MonoBehaviour
{   

    // How does this work? The enemy will cast two raycasts forward and a second one directly downwards from its front. If the downards ray detecs no ground or if the forward ray detects
    // a wall, the enemy will turn around. 

    // NOTE: There will be two default movements script that all enemies will have. This script will include directions, create all raycasts and have default functions
    // The only difference between the two is that one is for enemies that dont react to the player, the other one does react to player. 
    // Actual movement like walking will be done in separate scripts that will call the functions of the default scripts, and have the enemy move and react to different conditions. 


    private Rigidbody2D rb;
    

    [SerializeField] private float speed;
    int direction; // The only possible directions are left and right (-1 and 1) ****** The whole directions system will be reworked later ****************

    LayerMask groundLayer;
    private BoxCollider2D boxCollider;

    int enemyLayer; // ***** This will be placed in another script later. One script for movement that all enemies will have, followed by various scripts like walking, hopping, etc...

    // Start is called before the first frame update    
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");        
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer); // Prevents collisions between enemies

        groundLayer = LayerMask.GetMask("Ground");

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        switch (Random.Range(1, 3))
        {
            case 1: direction = 1; break;
            case 2: direction = -1; break;
        }

        
    }

    
    void Update()
    {
        //RaycastHit2D forward = Physics2D.Raycast(transform.position, forward, distance, groundLayer); // This raycast checks in front of the enemy. In this case, only for groundLayer, but can be changed to also check for player

        if (!isGrounded())
        {
            Flip();
        } else if (isWalled())
        {
            Flip();
        }

    }
    bool isGrounded()
    {
        // Getting position of downwards ray
        Vector2 ray_Position = transform.position;
        ray_Position.x += direction * (boxCollider.offset.x + boxCollider.size.x / 2); // Places the x position to the front of the enemy depending on direction
        ray_Position.y -= boxCollider.offset.y + boxCollider.size.y / 2;

        // Direction and distance of downwards ray
        Vector2 ray_Direction = Vector2.down;
        float ray_Distance = 0.1f;

        RaycastHit2D checkForGround = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, groundLayer);

        if (checkForGround.collider != null)
            return true;
        return false;

    }

    bool isWalled()
    {
        Vector2 ray_Position = transform.position;
        ray_Position.x += direction * (boxCollider.offset.x + boxCollider.size.x / 2);

        Vector2 ray_Direction = direction * Vector2.right;
        float ray_Distance = 0.05f;

        Vector2 ray_Size = new Vector2 (0.05f, boxCollider.size.y);

        RaycastHit2D checkForWall = Physics2D.BoxCast(ray_Position, ray_Size, 0.0f, ray_Direction, ray_Distance, groundLayer); // Box cast so that an obstacle at any height compared to the enemy is detected

        if (checkForWall.collider != null)
            return true;
        return false;
    }

    void Flip()
    {
        direction *= -1;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }
}
