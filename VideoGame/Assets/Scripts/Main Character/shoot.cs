using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    private float arrow_speed = 30f;

    private GameObject arrow;

    public itemInventory items;

    public Animator animator;

    private float nextAttackTime = 0f;
    public float attackRate = 1.8f;
   

    [SerializeField] private Transform attackpoint; 
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (arrow == null) return;

        
        

        if (Time.time >= nextAttackTime){
            if (Input.GetKeyUp(KeyCode.F)){
                Debug.Log("Shoot");
                Shoot();
                nextAttackTime = Time.time + 1f/attackRate;
            }

            
            
        }
    }


    public void Shoot()
    {
        if (attackpoint != null) Instantiate(arrow, attackpoint.position, attackpoint.rotation);
    
        Debug.Log("attacked");
    } 
}