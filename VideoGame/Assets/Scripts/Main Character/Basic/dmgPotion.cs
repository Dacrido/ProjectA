using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dmgPotion : MonoBehaviour
{
    shoot shoot_script;
    GameObject increaseText;
    // Start is called before the first frame update
    void Start()
    {
        shoot_script =  GameObject.FindGameObjectWithTag("Player").GetComponent<shoot>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damageIncrease(){
        
        if (shoot_script != null){
            shoot_script.curr_dmg = shoot_script.curr_dmg+0.70f;
                
            Destroy(this.gameObject);
        }
    }
}
