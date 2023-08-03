using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_Default_Walking : MonoBehaviour, IMovementScript // Maybe extend from general class? ***************************
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

    // Start is called before the first frame update    
    void Start()
    {
        needsLadder = false;
        isFlying = false;

        rb = GetComponent<Rigidbody2D>();
        General = GetComponent<Enemy_Behaviour>();

    }    

    void FixedUpdate()
    {
        rb.velocity = new Vector2(General.getDirection().x * speed, rb.velocity.y);
    }

    public float distanceFromGround()
    {
        return 0f;
    }
}
