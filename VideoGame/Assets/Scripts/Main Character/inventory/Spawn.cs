using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnObject;
    private Transform player;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public void SpawnDroppedItem()
    {
        Vector2 playerPos = new Vector2(player.position.x - 1f, player.position.y);
        Instantiate(spawnObject, playerPos, Quaternion.identity);
    }
}


