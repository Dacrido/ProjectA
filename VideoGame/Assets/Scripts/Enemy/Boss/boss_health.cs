using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class boss_health : MonoBehaviour
{
    public Slider slider;
    public int maxHealth;
    public int currHealth;

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

        enemyDeath();

        if (death) return;

        if (player_currPosition_x < minBound.transform.position.x) slider.gameObject.SetActive(false);

        else slider.gameObject.SetActive(true);

        if (currHealth <= 0) enemyDeath();        

        slider.value = currHealth ;

    }

    // Start is called before the first frame update
    public void takeDamage(int damage){
        currHealth -= damage;
    }

    public void enemyDeath(){
        if (currHealth <= 0) 
        {
            death = true;
            Destroy(gameObject);
        }
    }
}
