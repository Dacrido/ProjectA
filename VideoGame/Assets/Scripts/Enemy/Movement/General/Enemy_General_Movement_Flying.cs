using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_General_Movement_Flying : MonoBehaviour
{
    // This class contains general methods used by all the flying movement scripts. Most notably, raycasts, boolean methods, flip(), random directions, etc.
    // ********************* Must general flying class like ground class **********************
    LayerMask groundLayer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    int enemyLayer;
    float radius = 2.0f;

    private Vector2 movementDirection;
    [SerializeField] private float speed;

    // Start is called before the first frame update    
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer); // Prevents collisions between enemies ********************* Only needs to be done once, not in all instances. However, I might change how this work and add collisions. This is temporary for now

        groundLayer = LayerMask.GetMask("Ground");

        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        CalculateDirection();
    }

    void FixedUpdate()
    {
        rb.velocity = movementDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            print("Collision");
            CalculateDirection();
        }
    }

    void CalculateDirection() // Not random enough, causes problems
    {
        RaycastHit2D[] collisions = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0f, groundLayer);

        int minCollisions = int.MaxValue;
        Vector2 bestDirection = Vector2.zero;
        for (float angle = 0; angle < 360; angle += 10)
        {
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // The direction based on the given angle of a part of a circle. Cosine for x coordinate, sin for y coordinate

            int currentCollisions = 0;
            foreach(RaycastHit2D collision in collisions)
            {
                if (Vector2.Angle(collision.normal, direction) < 90)
                {
                    currentCollisions++;
                }
            }

            if (currentCollisions < minCollisions)
            {
                minCollisions = currentCollisions;
                bestDirection = direction;
            }
        }

        movementDirection = bestDirection.normalized;
    }

}
