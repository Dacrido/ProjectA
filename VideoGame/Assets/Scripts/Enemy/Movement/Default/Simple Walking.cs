using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWalking : MonoBehaviour
{

    [SerializeField] private float speed;
    private Directions prefered_Dir;
    
    // Start is called before the first frame update    
    void Start()
    {
        switch (Random.Range(1, 3))
        {
            case 1:
                prefered_Dir = Directions.Right; break;
            case 2:
                prefered_Dir = Directions.Left; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
