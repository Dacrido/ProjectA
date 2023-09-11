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

    private TextMeshProUGUI text_objective;
    public GameObject objective_UI;
    private TextMeshProUGUI text_completed;
    public GameObject collected_UI;

    public string collected_text;
    public string objective_text;

    public string[] objectives;
    public string[] completed;

    private int current_objective_num;

    public int mission_id;

    void Awake()
    {
        if (collected_UI != null){

            text_completed = collected_UI.GetComponent<TextMeshProUGUI>();
            text_objective = objective_UI.GetComponent<TextMeshProUGUI>();
            collected_text = text_completed.text;
            objective_text = text_completed.text;
        }
    }
    void Update(){

        localMission();

        if (objective_text != null) text_objective.text = "Current Objective: " + objective_text + objectives[current_objective_num];

        if (collected_UI != null && !objectiveCompleted) text_completed.text = completed[current_objective_num] + collected.ToString();

        else if (objectiveCompleted)
        {
            text_completed.text = "Objective Completed!\nHead to the exit";
            text_objective.text = "No more objectives left";
        }
    }

    public void Collect(){
        collected +=1;
    }

    void complete_objective() {
        current_objective_num++;

        if (current_objective_num == objectives.Length) {
            objectiveCompleted = true;
        }
    }

    void localMission() {

        if (mission_id == 1) // collect 4 crystals
        {
            required = 4;

            if (collected == required) { 
                complete_objective();
                collected = 0;
            }
        }

        else if (mission_id == 2) // kill 5 enemies
        {
            required = 5;
            if (collected == required) { 
                complete_objective();
                mission_id++;            
            }


        }

        else if (mission_id == 3) // kill the slime boss
        {
            
        }
        

    }
    
     
    
}
