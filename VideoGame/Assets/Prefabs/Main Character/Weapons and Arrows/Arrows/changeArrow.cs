using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeArrow : MonoBehaviour
{
    private GameObject player;
    private shoot shootScript;
    // Start is called before the first frame update
    void Awake()
    {
        player =  GameObject.FindGameObjectWithTag("Player");
        shootScript = player.GetComponent<shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeArrow(GameObject arrow)
    {
        shootScript.equipped_arrow = arrow;
    }

}
