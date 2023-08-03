using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Settings : MonoBehaviour
{

    /*
     * This script contains the general settings that are generalized for all enemy instances. (Only contains code that doesnt need to repeat per instance
     */


    int enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer); // prevents collisions between enemies. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IMovementScript // Interface for all movement scripts
{
    float minTime    { get; }
    float maxTime    { get; }
    bool canRepeat   { get; }
    bool needsLadder { get; }
    bool isFlying    { get; }

    float distanceFromGround(); 
}
