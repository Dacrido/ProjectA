using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Slider HealthBar;
    public int maxHealth;
    [HideInInspector] public int currentHealth;

    // Start is called before the first frame update
    void Start() // This is current health for enemies. When activated/spawned, whenever entering a new area, they start with max health
    {
        currentHealth = maxHealth;
        HealthBar = GetComponentInChildren<Slider>();
        HealthBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        
        if (HealthBar.gameObject.activeSelf && currentHealth == maxHealth) // If the health bar is active and the currentHealth is equal to the maxHealth, hide health bar after 3 seconds
        {
            StartCoroutine(Wait(2));
            HealthBar.gameObject.SetActive(false);
        }

        if (!HealthBar.gameObject.activeSelf && currentHealth != maxHealth)
        {
            HealthBar.gameObject.SetActive(true);
        }
        
    }

    IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void takeDamage(int damage)
    {   
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }
    
    public void healHealth(int health)
    {
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    
}
