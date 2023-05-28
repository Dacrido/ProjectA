using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{

    public Health health; // The 'object' the slider is attached to
    public Image fill; // What changes in the slider 
    private Slider slider; // The slider this script is attached to

    // Start is called before the first frame update
    void Awake() 
    {
        slider = GetComponent<Slider>(); // Getting the slider component the script is attached to
    }

    // Update is called once per frame
    public void UpdateHealthBar()
    {
        if (slider.value <= slider.minValue) // To remove the little remaining bit of the bar
        {
            fill.enabled = false;
        }

        if (!fill.enabled && slider.value > slider.minValue)
        {
            fill.enabled = true;
        }


        float fillAmount = (float) health.currentHealth / (float) health.maxHealth;
        slider.value = fillAmount;

        // Rotation for enemies and player is frozen, so there isn't any issue with keeping the health bar above enemies. Plus, it's a side view game
    }
}
