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

        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = movementDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CalculateDirection(collision.contacts[0].normal);
        }
    }

    void CalculateDirection(Vector2 collisionDirection) 
    {
        float angle = Random.Range(-45f, 45f);

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Getting a rotation of angle degrees. Vector3.forward is used as it rotates the angle in the XY plane as the Z-axis is perpendicular to this plane
        Vector2 direction = rotation * collisionDirection;

        movementDirection = direction;

    }

}
