using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player"){
            gameManager.Collect();
            Destroy(gameObject);
        }
    }
}
