using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Default_Hopping : MonoBehaviour, IMovementScript 
{


    // Public Conditions (required among all movement scripts)

    [SerializeField]
    private float _minTime;
    public float minTime
    {
        get { return _minTime; }
        set { _minTime = value; }
    }

    [SerializeField]
    private float _maxTime;
    public float maxTime
    {
        get { return _maxTime; }
        set { _maxTime = value; }
    }


    [SerializeField]
    private bool _canRepeat;
    public bool canRepeat
    {
        get { return _canRepeat; }
        set { _canRepeat = value; }
    }

    public bool needsLadder { get; set; }

    public bool isFlying { get; set; }


    // Private

    private Rigidbody2D rb;
    private Enemy_Behaviour General;

    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    private float startingHeight; // starting height before the jump

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        General = GetComponent<Enemy_Behaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (General.isGrounded())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(General.getDirection().x * speed, rb.velocity.y);
    }

    private void Jump()
    {   
        startingHeight = transform.position.y;
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    public float distanceFromGround()
    {
        return transform.position.y - startingHeight;
    }
}
