using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_General_Movement_Ground : MonoBehaviour
{

    // ****************** OLD CLASS NO LONGER USED
    // This class contains general methods used by all the ground movement scripts. Most notably, raycasts, boolean methods and flip()

    private float speed;
    private int direction; // The only possible directions are left and right (-1 and 1) ****** The whole directions system will be reworked later ****************
    private bool isChasing = false;
    private bool lockInPlace = false;

    LayerMask groundLayer;
    LayerMask playerLayer;
    private BoxCollider2D boxCollider;
    //[SerializeField] private Slider HealthBar;
    
    int enemyLayer;

    // Start is called before the first frame update    
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer); // Prevents collisions between enemies ********************* Only needs to be done once, not in all instances. However, I might change how this work and add collisions. This is temporary for now

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");

        boxCollider = GetComponent<BoxCollider2D>();        

        switch (Random.Range(1, 3))
        {
            case 1: direction = 1;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
                break;
            case 2: direction = -1; break;
        }

    }

    // extraRayDistance: extra distance added to the ray say if the enemy is jumping off the ground
    public bool isGrounded(float extraRayDistance = 0.0f) // Checks if the enemy is on the ground of not
    {
        // Getting position of downwards ray
        Vector2 ray_Position = transform.position;
        ray_Position.x += boxCollider.offset.x + direction * (boxCollider.size.x / 2); // Places the x position to the front of the enemy depending on direction
        ray_Position.y -= boxCollider.size.y / 2 - boxCollider.offset.y;

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
        ray_Position.x += boxCollider.offset.x + direction * (boxCollider.size.x / 2);
        //ray_Position.y += boxCollider.offset.y;

        Vector2 ray_Direction = direction * Vector2.right;
        float ray_Distance = 0.05f;

        Vector2 ray_Size = new Vector2(0.4f, boxCollider.size.y);

        RaycastHit2D checkForWall = Physics2D.BoxCast(ray_Position, ray_Size, 0.0f, ray_Direction, ray_Distance, groundLayer); // Box cast so that an obstacle at any height compared to the enemy is detected

        if (checkForWall.collider != null)
            return true;
        
        return false;
    }

    public bool seePlayer()
    {

        Vector2 ray_Position = transform.position;
        ray_Position.x += direction * (boxCollider.offset.x + boxCollider.size.x / 2);

        Vector2 ray_Direction = direction * Vector2.right;
        float ray_Distance = 4.0f;

        RaycastHit2D checkForPlayer = Physics2D.Raycast(ray_Position, ray_Direction, ray_Distance, playerLayer); 

        if (checkForPlayer.collider != null)
            return true;
        return false;
    }

    public void Flip() // Must be updated to physically flip the enemy, as well as not flip the health bar
    {
        direction *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void DefaultReaction(float extra = 0.0f) // Default movement reaction for enemies 
    {
        
        if (isChasing)
        {
            if (!isGrounded(extra) || isWalled())
            {
                lockInPlace = true;
            } else
            {
                lockInPlace = false;
            }

        } else
        {
            if (!isGrounded(extra) || isWalled())
            {
                Flip();
            }
        }
        
        

    }

    public int GetDirection() // Direction system will be reworked later. At the moment for ground enemies, -1 and 1 for left and right is fine. 
    {                         // However, when up and down are added, the whole system will be changed. 
        return direction;
    }

    public bool GetisChasing()
    {
        return isChasing;
    }

    public void SetisChasing(bool isChasing)
    {
        this.isChasing = isChasing;
    }

    public bool GetLock()
    {
        return lockInPlace;
    }

    public void SetLock(bool lockInPlace) { 
        this.lockInPlace = lockInPlace;
    }

}
