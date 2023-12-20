using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    bool trap_activated = false;
    public Animator anim;
    GameObject player;
    player_health health;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) health = player.GetComponent<player_health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
    void OnTriggerEnter2D(Collider2D obj) {
        if (anim != null && obj.gameObject.tag == "Player")
        {
            trap_activated = true;
            anim.SetTrigger("activated");
        }

        else trap_activated = false;

    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            Damage();
            
        }

    }

    void Damage() {
        if (health != null) {
            health.takeDamage(1);
        }
    }

    void DestroySelf() {
        Destroy(gameObject);
    }




}
