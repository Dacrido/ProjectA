using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public Canvas canvas;
    private float additionalOffset = 0.2f;

    private Dictionary<GameObject, Slider> healthBars = new Dictionary<GameObject, Slider>();

    void Start()
    {
        
    }

    void Update()
    {
        foreach (KeyValuePair<GameObject, Slider> kvp in healthBars)
        {
            GameObject enemy = kvp.Key;
            Slider healthBar = kvp.Value;

            SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
            float yOffset = enemySprite.bounds.extents.y + additionalOffset;


            healthBar.transform.position = enemy.transform.position + new Vector3(0, yOffset, 0);
        }
    }

    public void CreateHealthBar(GameObject enemy)
    {
        GameObject healthBarObject = Instantiate(healthBarPrefab); // Instantiating the health bar object
        healthBarObject.transform.SetParent(canvas.transform); // Must set the object parent to the canvas in order for it to be displayed (it is a UI object)
        
        Slider healthBar = healthBarObject.GetComponent<Slider>(); // Getting the slider
        FillStatusBar HealthBarCode = healthBar.GetComponent<FillStatusBar>(); // Getting the code of the slider health bar
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>(); // Getting the health of the enemy

        HealthBarCode.SetHealth(enemyHealth); // Giving the enemy health information to the code of the health bar slider
        healthBars.Add(enemy, healthBar); // Adding it to the dictionary
        healthBar.transform.localScale = new Vector2(0.8f, 1f);

    }

    public void RemoveHealthBar(GameObject enemy)
    {
        if (healthBars.ContainsKey(enemy))
        {
            healthBars.Remove(enemy);
        }
    }
}
