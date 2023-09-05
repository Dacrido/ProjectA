using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    
    private int collected;
    public int required = 4;
    public bool objectiveCompleted = false;
    private GameObject player;
    private itemInventory inventory;


    void Update(){
        if (collected == required) objectiveCompleted = true; 
        
    }

    public void Collect(){
        collected +=1;
    }
    
    
    
     
    
}
