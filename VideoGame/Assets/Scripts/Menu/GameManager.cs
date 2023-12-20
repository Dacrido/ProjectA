using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{

    
    private int collected;
    public int required;
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

    private GameObject[] enemies;
    private GameObject[] aliveEnemies;

    public int mission_id;

    public GameObject boss;

    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (collected_UI != null){

            text_completed = collected_UI.GetComponent<TextMeshProUGUI>();
            text_objective = objective_UI.GetComponent<TextMeshProUGUI>();
            collected_text = text_completed.text;
            objective_text = text_objective.text;
        }

    }
    void Update(){
        if (!objectiveCompleted) localMission();

        if (objectiveCompleted && mission_id != 0)
        {
            text_completed.text = "Objective Completed! Head to the exit";
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
        
        if (objective_text != null && !objectiveCompleted) text_objective.text = "Current Objective: " + objective_text + objectives[current_objective_num];

        if (collected_UI != null && !objectiveCompleted && mission_id != 0) text_completed.text = completed[current_objective_num] + collected.ToString();
        
        if (mission_id == 0){
            objectiveCompleted = true;
        }

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
            
            collected = enemies.Length - getEnemies("Enemy");

            if (collected == required) { 
                complete_objective();
                mission_id++;            
            }


        }

        else if (mission_id == 3) // kill the slime boss
        {
            text_completed.text = completed[current_objective_num];
            boss.SetActive(true);
            if (getEnemies("Boss") == 0 && getEnemies("SpawnerMini") == 0){
                complete_objective();
            }

            
        }
        

    }

    int getEnemies(string type){
        aliveEnemies = GameObject.FindGameObjectsWithTag(type);
        return aliveEnemies.Length;
    }
     
    
}
