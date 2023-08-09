using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IDLE : MonoBehaviour, IMovementScript
{
    public float minTime => throw new System.NotImplementedException();

    public float maxTime => throw new System.NotImplementedException();

    public bool canRepeat => throw new System.NotImplementedException();

    public bool needsLadder => throw new System.NotImplementedException();

    public bool isFlying => throw new System.NotImplementedException();

    public float distanceFromGround()
    {
        return 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
