using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    private float speed = 600.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.right * speed);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
