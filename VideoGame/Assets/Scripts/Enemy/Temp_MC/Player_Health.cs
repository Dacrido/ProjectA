using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    
    public int maxHealth;
    [HideInInspector] public int currentHealth;

    // Invincibility
    private float invincibileDuration = 1.5f;
    private bool isInvincible = false;
    private SpriteRenderer Image;

    // Start is called before the first frame update
    void Start() // This is current health for enemies. When activated/spawned, whenever entering a new area, they start with max health
    {
        currentHealth = maxHealth;
        Image = GetComponent<SpriteRenderer>();
    }

    private IEnumerator temporaryInvincibility(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private IEnumerator flashInvincility(float duration)
    {
        float interval = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Image.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(interval);
            Image.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(interval);
            elapsedTime += interval * 2;

        }

    }

    public void takeDamage(int damage)
    {
        
        if (isInvincible) { return; }
        
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        
        StartCoroutine(temporaryInvincibility(invincibileDuration)); // 1 second of invincibility for each hit.
        StartCoroutine(flashInvincility(invincibileDuration));

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
