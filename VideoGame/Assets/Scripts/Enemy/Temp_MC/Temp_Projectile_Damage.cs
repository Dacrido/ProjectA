using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Projectile_Damage : MonoBehaviour
{
    public int damage;
    private bool hasCollided = false; // This boolean is used to prevent one projectile causing damage among multiple enemies if the enemies are occupying the same space at that instant. 
    private void OnCollisionEnter2D(Collision2D collision) // Private is on by default
    {
        if (collision.gameObject.tag == "Enemy" && !hasCollided)
        {
            hasCollided = true; // The first collision makes this variable true. If a second collision occurs before the projectile is destroyed, this variable prevents anything from happening
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>(); // Decrease health
            health.takeDamage(damage);

            Destroy(gameObject); // Destroy projectile

            

        }
    }
}
