 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Default_Flying : MonoBehaviour, IMovementScript
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

    private int counter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        needsLadder = false;
        isFlying = true;

        rb = GetComponent<Rigidbody2D>();
        General = GetComponent<Enemy_Behaviour>();
    }

    private void OnEnable()
    {
        if (rb != null)
            rb.gravityScale = 1;

        counter = 0;
    }

    void FixedUpdate()
    {
        rb.velocity = General.getDirection() * speed;
    }

    // For some reason, onCollisions still run even if the script itself is disabled ****************************************
    private void OnCollisionStay2D(Collision2D collision) // Stay not Enter as if enemy is in idle and colliding, enter isnt called in this script
    {
        if (!enabled)
            return;

        if (counter == 0)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                CalculateDirection(collision.contacts[0].normal);
            }
        }

        counter++;

        if (counter >= 3)
            counter = 0;
        
    }

    void CalculateDirection(Vector2 collisionDirection)
    {
        float angle = Random.Range(-45f, 45f);

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Getting a rotation of angle degrees. Vector3.forward is used as it rotates the angle in the XY plane as the Z-axis is perpendicular to this plane
        Vector2 direction = rotation * collisionDirection;

        General.setDirection(direction);

    }

    public float distanceFromGround()
    {
        return 0f;
    }
}
