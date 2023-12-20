using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    private float arrow_speed = 30f;

    public float curr_dmg = 1;

    public GameObject default_arrow;
    public GameObject equipped_arrow;


    public Animator animator;

    private float nextAttackTime = 0f;
    public float attackRate = 1.8f;
   
    public arrow container;

    [SerializeField] private Transform attackpoint; 
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (equipped_arrow == null) 
        {
            equipped_arrow = default_arrow;
        }
        
        

        if (Time.time >= nextAttackTime){
            if (Input.GetKeyUp(KeyCode.F)){
                StartCoroutine(Shoot());
                nextAttackTime = Time.time + 1f/attackRate;
            }

            
            
        }
    }
    public IEnumerator Shoot(){
        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(20f/60f);
        ReleaseArrow();

    }

    public void ReleaseArrow()
    {
        if (attackpoint != null) {
            Instantiate(equipped_arrow, attackpoint.position, attackpoint.rotation);
        }

    } 

    
}