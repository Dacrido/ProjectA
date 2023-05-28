using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private health playerHealth;
    [SerializeField] private Image  total_healthBar;
    [SerializeField] private Image  current_healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        total_healthBar.fillAmount = playerHealth.current_health;
    }

    // Update is called once per frame
    void Update()
    {
        current_healthBar.fillAmount = playerHealth.current_health;
    }
}
