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
    private SpriteRenderer[] Images;

    int playerLayer;
    int enemyLayer;

    // Start is called before the first frame update
    void Start() // This is current health for enemies. When activated/spawned, whenever entering a new area, they start with max health
    {   
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        currentHealth = maxHealth;
        Images = GetComponentsInChildren<SpriteRenderer>(); // Gets all the spirte Renderer components (parent and children)
    }

    private IEnumerator temporaryInvincibility(float duration)
    {
        isInvincible = true;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer);
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }

    private IEnumerator flashInvincility(float duration)
    {
        float interval = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {   
            foreach (SpriteRenderer Image in Images)
            {
                Color color = Image.color;
                color.a = 0f; // The opaqueness of the image
                Image.color = color;
            }
            
            yield return new WaitForSeconds(interval);

            foreach (SpriteRenderer Image in Images)
            {
                Color color = Image.color;
                color.a = 1f;
                Image.color = color;
            }            
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
