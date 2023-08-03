using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{

    public EnemyHealth health; // The 'object' the slider is attached to
    public Image fill; // What changes in the slider 
    private Slider slider; // The slider this script is attached to


    void Start()
    {
        //Hide();
    }

    public void SetHealth(EnemyHealth enemyHealth)
    {
        health = enemyHealth;
        slider = GetComponentInParent<Slider>();
        health.onHealthChange.AddListener(UpdateHealthBar);
        health.deleteHealthBar.AddListener(DeleteHealthBar);
        slider.gameObject.SetActive(false);
    }

    IEnumerator Hiding(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        slider.gameObject.SetActive(false);
    }    

    public void UpdateHealthBar()
    {   

        // The following isnt needed as when the health bar reaches 0, the enemy is deleted and so is the health bar. Serves no purpose. 
        /*if (slider.value <= slider.minValue) // To remove the little remaining bit of the bar.
        {
            fill.enabled = false;
        }

        if (!fill.enabled && slider.value > slider.minValue)
        {
            fill.enabled = true;
        }*/

        if (slider.gameObject.activeSelf && health.currentHealth == health.maxHealth)
        {
            Hiding(2);
        }

        if (!slider.gameObject.activeSelf && health.currentHealth != health.maxHealth)
        {
            slider.gameObject.SetActive(true);
        }

        float fillAmount = (float) health.currentHealth / (float) health.maxHealth;
        slider.value = fillAmount;

        // Rotation for enemies and player is frozen, so there isn't any issue with keeping the health bar above enemies. Plus, it's a side view game
    }

    private void DeleteHealthBar()
    {
        Destroy(slider.gameObject);
    }


}
