using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    private HealthBarManager healthManager;

    public int maxHealth;
    [HideInInspector] public int currentHealth;
    public UnityEvent onHealthChange;
    public UnityEvent deleteHealthBar;

    // Start is called before the first frame update
    void Start() // This is current health for enemies. When activated/spawned, whenever entering a new area, they start with max health
    {

        healthManager = FindObjectOfType<HealthBarManager>();
        healthManager.CreateHealthBar(gameObject);
        
        currentHealth = maxHealth;
        onHealthChange.Invoke();
    }

    public void takeDamage(int damage)
    {   
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            deleteHealthBar.Invoke();
            healthManager.RemoveHealthBar(gameObject);
            Destroy(gameObject);
        }

        onHealthChange.Invoke();

    }
    
    public void healHealth(int health)
    {
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        onHealthChange.Invoke();
    }
    
}
