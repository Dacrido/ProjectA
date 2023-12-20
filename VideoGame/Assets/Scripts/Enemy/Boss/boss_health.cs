using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class boss_health : MonoBehaviour
{
    public Slider slider;
    public float maxHealth;
    public float currHealth;

    [System.NonSerialized] public bool death = false;

    [System.NonSerialized] public bool hit;
    
    Color originalColor;
    public SpriteRenderer m_SpriteRenderer;
    
    public Transform minBound;
    public GameObject player;
    private float player_currPosition_x;

    void Awake()
    {
        currHealth = maxHealth;
        originalColor = m_SpriteRenderer.color;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    public void Update(){
        player_currPosition_x = player.transform.position.x;


        if (death) return;

        if (currHealth <= 0) StartCoroutine(enemyDeath());        

        slider.value = currHealth ;

    }

    // Start is called before the first frame update
    public void takeDamage(float damage){
        currHealth -= damage;
    }

    public IEnumerator enemyDeath(){
        
        death = true;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        
    }
}
