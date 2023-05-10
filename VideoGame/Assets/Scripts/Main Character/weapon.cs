using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{

    public Transform attackpoint;
    public GameObject arrowPrefab;
    public GameObject chargedArrow;
    
    private float charge_time = 0.7f;
    private float time_counter;

    public GameObject player;
    private controls other;

    void Awake(){
        GameObject player = GameObject.Find("player_default");     
        other = player.GetComponent<controls>(); 
    }
    


    // Start is called before the first frame update
    
    void Start()
    {
        time_counter = 0f;
    }




    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.F)){
            time_counter+= Time.deltaTime;
            
        }

        else if (Input.GetKeyUp(KeyCode.F)){
            if (time_counter > charge_time) SpawnArrowHeavy();

            else SpawnArrowLight();

            time_counter = 0;
            
        }
       

    }

    void SpawnArrowLight(){
        
        Instantiate(arrowPrefab, attackpoint.position, attackpoint.rotation);
        
    }

    void SpawnArrowHeavy(){
        
        Instantiate(chargedArrow, attackpoint.position, attackpoint.rotation);
        
    }


}
