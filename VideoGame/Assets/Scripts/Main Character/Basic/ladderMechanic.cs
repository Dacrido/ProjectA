using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderMechanic : MonoBehaviour
{
    private float vertical;
    public float speed = 8f;
    private bool isLadder;
    private bool isClimbing;
    private float originalGScale;
    [SerializeField] private Rigidbody2D rb;
    void Start()
    {
        originalGScale = rb.gravityScale;
    }
    
    void Update(){
        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate() {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical*speed);
        }    

        else rb.gravityScale = originalGScale;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ladder")){
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ladder")){
            isLadder = false;
            isClimbing = false;
            
        }
    }
}
