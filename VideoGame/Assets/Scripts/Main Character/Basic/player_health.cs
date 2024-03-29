using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class player_health : MonoBehaviour
{
    
    
    SpriteRenderer m_SpriteRenderer;
    Color originalColor;
    
    private const int maxHEALTH = 5;

    public int HEALTH;
    public Image[] lives;
    public Sprite fullLives;
    public Sprite noLives;

    public GameObject youDied;
    // Start is called before the first frame update
    
    private float damage_cd;
    private float time_counter;

    private void Awake()
    {
        HEALTH = maxHEALTH;
    }
    
    
    void Start()
    {
        HEALTH = maxHEALTH;  
        damage_cd = 1f;
        time_counter = 0f;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = m_SpriteRenderer.color;  
    }

    // Update is called once per frame
    void Update()
    {
        if (HEALTH > maxHEALTH) HEALTH = 5; 
        
        if (time_counter <= damage_cd)
        {
            time_counter += Time.deltaTime;
        }

        if (HEALTH <= 0f) 
        {
            Debug.Log("You Died!");
            youDied.SetActive(true);
            gameObject.active = false;

        }

        for (int i = 0; i < lives.Length; i++){
            
            if (i < HEALTH){
                lives[i].sprite = fullLives;
            } else {
                lives[i].sprite = noLives;
            }
            
            
            
        }
        

    }


    public void Heal(int heal)
    {
        HEALTH += heal;
    }

    public void takeDamage(int damage)
    {
        
        if (time_counter > damage_cd) 
        {
            HEALTH -= damage;       
            time_counter = 0f;
            StartCoroutine(changeColor());
        }
    }

    IEnumerator changeColor(){  
        Color customColor = HexToColor("#FF9894");
        m_SpriteRenderer.color = customColor;
        yield return new WaitForSeconds(0.3f);
        m_SpriteRenderer.color = originalColor;
    }

    Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}
