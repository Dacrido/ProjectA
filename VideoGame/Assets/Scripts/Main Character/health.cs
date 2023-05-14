using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public float num_lives;
    // Start is called before the first frame update
    void Start()
    {
        num_lives = 5;    
    }

    // Update is called once per frame
    void Update()
    {
        if (num_lives < 1) {
            Debug.Log("You Died!");
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D enemy) {
        if (enemy.gameObject.tag == "Enemy") {
            num_lives -= 1;
            Debug.Log("Lives remaining" + num_lives);
        }

    }

}
