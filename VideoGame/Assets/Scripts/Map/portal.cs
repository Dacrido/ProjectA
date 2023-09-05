using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class portal : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]public bool collided = false;
    public GameObject text;

    public int Level;

    public GameManager gameManager;


    private void Update(){

        if (text != null){
            
            if (collided) text.gameObject.SetActive(true);
            
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
