using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Default_Walking : MonoBehaviour
{   

    // How does this work? The enemy will cast two raycasts forward and a second one directly downwards from its front. If the downards ray detecs no ground or if the forward ray detects
    // a wall, the enemy will turn around. 

    // NOTE: There will be two default movements script that all enemies will have. This script will include directions, create all raycasts and have default functions
    // The only difference between the two is that one is for enemies that dont react to the player, the other one does react to player. 
    // Actual movement like walking will be done in separate scripts that will call the functions of the default scripts, and have the enemy move and react to different conditions. 


    private Rigidbody2D rb;
    private Enemy_General_Movement_Ground General;

    [SerializeField] private float speed;

    // Start is called before the first frame update    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        General = GetComponent<Enemy_General_Movement_Ground>();

    }

    
    void Update()
    {
        General.DefaultReaction(0.0f);
    }

    

    void FixedUpdate()
    {
        rb.velocity = new Vector2(General.GetDirection() * speed, rb.velocity.y);
    }
}
