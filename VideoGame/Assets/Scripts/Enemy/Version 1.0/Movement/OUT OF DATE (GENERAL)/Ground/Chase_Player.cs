using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Chase_Player : MonoBehaviour
{
    // This is a temporary follow player solution. This will be updated later

    private Transform target;
    private Enemy_General_Movement_Ground General;
    
    [SerializeField] private float followTime;
    private float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        General = GetComponent<Enemy_General_Movement_Ground>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (General.seePlayer())
        {
            currentTime = 0.0f;
            General.SetisChasing(true);
        } else
        {
            currentTime += Time.deltaTime;

            if (currentTime > followTime)
            {
                General.SetisChasing(false);
                General.SetLock(false);
            }
        }

        if (General.GetisChasing())
            Chase();
            
    
    }

    void Chase()
    {        
        int direction = 0;
        float difference = target.position.x - transform.position.x;
        if (difference < 0)
        {
            direction = -1;
        }
        else if (difference > 0)
        {
            direction = 1;
        } else if (difference == 0)
        {
            General.SetLock(true);
            return;
        }

        if (direction != General.GetDirection()) { General.Flip(); }
        
    }


    


}
