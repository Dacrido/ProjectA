using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{

    public Transform attackpoint;
    public GameObject arrowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            ShootArrow();
        }
    }

    void ShootArrow(){
        
        Instantiate(arrowPrefab, attackpoint.position, attackpoint.rotation);
        
    }


}
