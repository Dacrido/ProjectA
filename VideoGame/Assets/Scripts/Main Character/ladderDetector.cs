using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderDetector : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ladder>())
        {
            player.ClimbingAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ladder>())
        {
            player.ClimbingAllowed = false;
        }
    }
}
