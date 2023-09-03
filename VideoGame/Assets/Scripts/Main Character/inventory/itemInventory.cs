using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemInventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;

    public bool call = false;
    
    private void Update(){
        if (call){
            for (int i = 0; i < slots.Length; i++){
            isFull[i] = GameManager.Instance.isFull[i];
            slots[i] = GameManager.Instance.slots[i].gameObject;

        }
        }

    }

}
