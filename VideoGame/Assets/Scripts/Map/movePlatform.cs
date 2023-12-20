using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public bool horizontal_mode = true;

    public Transform upper_bound;
    public Transform lower_bound;

    int direction = 1;
    public float speed = 3f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        checkDirection();
        float moveAmount = speed * direction ;

        if (horizontal_mode)
            rb.velocity = new Vector2(moveAmount, rb.velocity.y);
        else
            rb.velocity = new Vector2(rb.velocity.x, moveAmount);
    }

    void checkDirection()
    {
        if (horizontal_mode){
            if (transform.position.x > upper_bound.position.x){
                direction = -1;
            } 

            if (transform.position.x < lower_bound.position.x)
            {
                direction = 1;
            }
        }

        else {
            if (transform.position.y > upper_bound.position.y){
                direction = -1;
            } 

            if (transform.position.y < lower_bound.position.y)
            {
                direction = 1;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(this.transform, true);
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Player"){
            collision.gameObject.transform.SetParent(null);
        }
    }
}
