using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bow : MonoBehaviour
{
    // shooting
    public Transform attackpoint;
    public Transform attackpoint2;
    public Transform attackpoint3;

    public GameObject arrowPrefab;
    public GameObject chargedArrow;
    
    public Transform inventory;
    
    private float nextAttackTime = 0f;
    private float attackRate = 1.3f;

    private float charge_time = 0.7f;
    private float time_counter_attack;


    private bool multiArrow = false; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        time_counter_attack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory == null) return;
        
        if (transform.IsChildOf(inventory))
        {
            if (Time.time >= nextAttackTime) // if the time is more than the 
            {
                if (Input.GetKey(KeyCode.F)){
                    time_counter_attack += Time.deltaTime;
                }

                
                else if (Input.GetKeyUp(KeyCode.F)){
                    if (time_counter_attack > charge_time) SpawnArrowHeavy();

                    else SpawnArrowLight();
                    

                    nextAttackTime = Time.time + 1f/attackRate;
                    time_counter_attack = 0;
                
                }
            }

            if (Input.GetKeyUp(KeyCode.X))  multiArrow = !multiArrow;
        }
    }

    void SpawnArrowLight(){
        
        if (multiArrow){
            Instantiate(arrowPrefab, attackpoint.position, attackpoint.rotation); // spawn arrows at attackpoint
            Instantiate(arrowPrefab, attackpoint2.position, attackpoint.rotation);
            Instantiate(arrowPrefab, attackpoint3.position, attackpoint.rotation);
            return;
        }
            
        Instantiate(arrowPrefab, attackpoint.position, attackpoint.rotation);

    }

    void SpawnArrowHeavy(){
        
        Instantiate(chargedArrow, attackpoint.position, attackpoint.rotation);       
    }

    


}
