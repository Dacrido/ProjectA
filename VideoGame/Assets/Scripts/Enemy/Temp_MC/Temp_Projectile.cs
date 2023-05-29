using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    private float speed = 600.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.right * speed);
        Destroy(gameObject, 1.5f); // Projectile despawns if either 1.5 seconds passed, collides with the ground layer, or hits an enemy
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // 6 is ground layer
        {
            Destroy(gameObject);
        }
    }
}
