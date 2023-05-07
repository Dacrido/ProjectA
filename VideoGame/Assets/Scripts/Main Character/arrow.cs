using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 20f;

    public Rigidbody2D rb;
    public GameObject impact;
    [SerializeField] controls ctrlPlayer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (ctrlPlayer.isFacingRight == true) rb.velocity = new Vector2(speed, rb.velocity.y);
        
        if (ctrlPlayer.isFacingRight == false) rb.velocity = new Vector2(-speed, rb.velocity.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
