using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bow_2 : MonoBehaviour
{
    public Transform attackpoint;   
    public GameObject chargedArrow;
    private float nextAttackTime = 0f; 
    private float attackRate = 2f;

    private float charge_time = 0.7f;
    private float time_counter;

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
            if (Input.GetKey(KeyCode.F)){
                time_counter+= Time.deltaTime;       
            }

            else if (Input.GetKeyUp(KeyCode.F)){
                if (time_counter > charge_time) Shoot();
                time_counter = 0;     
            }
        }
    }

    public void Shoot()
    {
        Instantiate(chargedArrow, attackpoint.position, attackpoint.rotation);
    }

    


}
