using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IDLE : MonoBehaviour, IMovementScript
{

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

    public bool canRepeat => throw new System.NotImplementedException();

    public bool needsLadder => throw new System.NotImplementedException();

    public bool isFlying => throw new System.NotImplementedException();

    public float distanceFromGround()
    {
        throw new System.NotImplementedException();
    }


    private Rigidbody2D rb;
    private Enemy_Behaviour General;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        General = rb.GetComponent<Enemy_Behaviour>();
    }

    private void OnEnable()
    {
        if (rb != null && General.isFlying())
            rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;
    }
}
