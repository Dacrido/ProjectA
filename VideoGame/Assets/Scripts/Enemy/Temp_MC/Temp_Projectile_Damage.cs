using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Projectile_Damage : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision) // Private is on by default
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Health health = collision.gameObject.GetComponent<Health>(); // Decrease health
            health.takeDamage(damage);

            Destroy(gameObject); // Destroy projectile

            

        }
    }
}
