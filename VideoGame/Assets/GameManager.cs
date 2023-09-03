using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private GameObject player;
    private itemInventory inventory;

    public bool[] isFull;
    public GameObject[] slots;

    void Awake(){

        Debug.Log("Loaded");
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<itemInventory>();

        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }
        else{
            
            Instance = this;
            DontDestroyOnLoad(gameObject);    
        }
        
    }
    void Update(){
        
        
    }
    public void updateInventory(){
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<itemInventory>();
        
        for (int i = 0; i < slots.Length; i++){
            isFull[i] = inventory.isFull[i];
            slots[i] = inventory.slots[i].gameObject;

        }

    }
    
     
    
}
