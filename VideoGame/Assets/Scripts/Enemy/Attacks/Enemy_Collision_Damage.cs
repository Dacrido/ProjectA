using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collision_Damage : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision) // Private is on by default
    {
        if (collision.gameObject.tag == "Player")
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.takeDamage(damage);
        }
    }
}
