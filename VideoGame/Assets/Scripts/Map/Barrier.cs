using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    BoxCollider2D boxCollider;
    
    public Transform bound;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        m_SpriteRenderer.enabled = false; 
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        if (player.transform.position.x > (bound.position.x))
        {
            m_SpriteRenderer.enabled = true; 
            boxCollider.enabled = true; 
             
        }

        else {
            m_SpriteRenderer.enabled = false; 
            boxCollider.enabled = false; 
        
        }
        

    }
}
