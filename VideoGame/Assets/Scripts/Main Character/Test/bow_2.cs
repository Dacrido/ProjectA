using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bow_2 : MonoBehaviour
{
    public Transform attackpoint;   
    public GameObject chargedArrow;
    private float nextAttackTime = 0f; 
    private float attackRate = 2f;

    public Transform inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory == null) return;
        
        if (transform.IsChildOf(inventory))
        {
            if (Time.time >= nextAttackTime) 
            {                              
                if (Input.GetKeyUp(KeyCode.F))
                {
                    Shoot();
                    nextAttackTime = Time.time + 1f/attackRate;
          
                }
            } 
        }
    }

    public void Shoot()
    {
        Instantiate(chargedArrow, attackpoint.position, attackpoint.rotation);
    }

    


}
