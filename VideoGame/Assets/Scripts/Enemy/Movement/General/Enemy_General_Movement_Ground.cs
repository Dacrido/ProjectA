using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_General_Movement_Ground : MonoBehaviour
{

    // This class contains general methods used by all the movement scripts. Most notably, raycasts, boolean methods and flip()

    private int direction; // The only possible directions are left and right (-1 and 1) ****** The whole directions system will be reworked later ****************

    LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    
    int enemyLayer;

    // Start is called before the first frame update    
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer); // Prevents collisions between enemies (Might be changed later)

        groundLayer = LayerMask.GetMask("Ground");

        boxCollider = GetComponent<BoxCollider2D>();

        switch (Random.Range(1, 3))
        {
            case 1: direction = 1; break;
            case 2: direction = -1; break;
        }

    }

    // extraRayDistance: extra distance added to the ray say if the enemy is jumping off the ground
    public bool isGrounded(float extraRayDistance = 0.0f) // Checks if the enemy is on the ground of not
    {
        // Getting position of downwards ray
        Vector2 ray_Position = transform.position;
        ray_Position.x += direction * (boxCollider.offset.x + boxCollider.size.x / 2); // Places the x position to the front of the enemy depending on direction
        ray_Position.y -= boxCollider.offset.y + boxCollider.size.y / 2;

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
        ray_Position.x += direction * (boxCollider.offset.x + boxCollider.size.x / 2);

        Vector2 ray_Direction = direction * Vector2.right;
        float ray_Distance = 0.05f;

        Vector2 ray_Size = new Vector2(0.05f, boxCollider.size.y);

        RaycastHit2D checkForWall = Physics2D.BoxCast(ray_Position, ray_Size, 0.0f, ray_Direction, ray_Distance, groundLayer); // Box cast so that an obstacle at any height compared to the enemy is detected

        if (checkForWall.collider != null)
            return true;
        return false;
    }

    public void Flip()
    {
        direction *= -1;
    }

    public void DefaultReaction(float extra = 0.0f) // This method might have to change
    {
        if (!isGrounded(extra))
        {
            Flip();
        }
        else if (isWalled())
        {
            Flip();
        }
    }

    public int GetDirection()
    {
        return direction;
    }
}
