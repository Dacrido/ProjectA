using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Collision_Damage : MonoBehaviour
{
    public int damage;
    private void OnCollisionStay2D(Collision2D collision) // Private is on by default
    {
        if (collision.gameObject.tag == "Player")
        {
            player_health playerHealth = collision.gameObject.GetComponent<player_health>();
            playerHealth.takeDamage(damage);
        }
    }
}
