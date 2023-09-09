using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class portal : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]public bool collided = false;
    public GameObject text;
    public TextMeshPro text_t;
    public int Level;

    public GameManager gameManager;

    void Awake()
    {
        text_t = text.GetComponent<TextMeshPro>();
    }


    private void Update(){

        if (text != null){
            
            if (collided) {       
                if (!gameManager.objectiveCompleted)
                {
                    text_t.text = "Objective Not Completed";
                }

                else if (gameManager.objectiveCompleted)
                {
                    text_t.text = "Press E to enter the portal";
                }
                
                text.gameObject.SetActive(true);
            
            }
            
            else text.gameObject.SetActive(false);   
        }

        if (collided && Input.GetKeyDown(KeyCode.E))
        {
            if (Level != null) {  
                
                if (gameManager.objectiveCompleted) LoadLevel(Level);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D col){
        
        if (col.gameObject.tag == "Player"){
            collided = true;
            

        }
    }
    public void OnTriggerExit2D(Collider2D col){
        if (col.gameObject.tag == "Player"){
            collided =false;
 
        }
    }

    public void LoadLevel(int sceneANumber)
    {
        Debug.Log("sceneBuildIndex to load: " + sceneANumber);
        SceneManager.LoadScene(sceneANumber+1);
    }

    IEnumerator Wait(float time){
        yield return new WaitForSeconds(time);
    }
    
}
