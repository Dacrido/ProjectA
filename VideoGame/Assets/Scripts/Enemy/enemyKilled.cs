using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyKilled : MonoBehaviour
{
    
    EnemyHealth enemyHealth; 
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemyHealth.currentHealth <= 0)
        {
            gameManager.Collect();
            
        }
        
    }
}
