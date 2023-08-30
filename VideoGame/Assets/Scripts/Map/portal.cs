using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    // Start is called before the first frame update
    public bool collided = false;
    public GameObject text;
    private void Update(){
        if (text != null){
            
            if (collided) text.gameObject.SetActive(true);
            
            else text.gameObject.SetActive(false);     
        }

        if (collided && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("next");
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
    
}
