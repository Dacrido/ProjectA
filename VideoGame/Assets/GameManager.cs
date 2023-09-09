using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{

    
    private int collected;
    public int required = 4;
    public bool objectiveCompleted = false;
    private GameObject player;
    private itemInventory inventory;


    public GameObject objective_UI;
    public TextMeshProUGUI text;
    public GameObject collected_UI;

    public string objective_text;

    void Awake()
    {
        if (collected_UI != null){

            text = collected_UI.GetComponent<TextMeshProUGUI>();
            objective_text = text.text;
        }
    }
    void Update(){
        if (collected == required) {
            objectiveCompleted = true;
        } 
        
        if (collected_UI != null && !objectiveCompleted) text.text = objective_text + collected.ToString();
        else if (objectiveCompleted) text.text = "Objective Completed!\nHead to the exit";
    }

    public void Collect(){
        collected +=1;
    }
    
    
    
     
    
}
