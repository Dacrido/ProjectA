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
            Player_Health health = collision.gameObject.GetComponent<Player_Health>();
            health.takeDamage(damage);
        }
    }
}
