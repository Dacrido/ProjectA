using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    private GameObject player;
    private shoot shootScript;
    private itemInventory inventory;
    private Button btn;
    
    public KeyCode UseKey;
    public KeyCode DropKey;
    public int i;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<itemInventory>();
        shootScript = player.GetComponent<shoot>();

    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;     
        }

        else
        {
            btn = transform.GetChild(0).GetComponent<Button>();
        }

        if (btn != null && Input.GetKeyDown(UseKey)){
            FadeToColor(btn.colors.pressedColor);
            btn.onClick.Invoke();
        }

        else if (btn != null && Input.GetKeyUp(UseKey))
        {
            FadeToColor(btn.colors.normalColor);
        }

        if (btn != null && Input.GetKeyDown(DropKey)){
            DropItem();
        }
        
        
    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, btn.colors.fadeDuration, true, true);
    }

    public void DropItem(){
        foreach (Transform child in transform)
        {
            child.GetComponent<Spawn>().SpawnDroppedItem();
            
            if (child.gameObject.tag == "Arrow") shootScript.equipped_arrow = shootScript.default_arrow;

            GameObject.Destroy(child.gameObject);
        }
    }
}
