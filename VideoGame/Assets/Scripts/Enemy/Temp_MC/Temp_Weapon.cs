using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(projectile, transform.position, transform.rotation);
            
        }
    }
}
