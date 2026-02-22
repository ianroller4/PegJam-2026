using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gob;

    private bool canSpawn = true;

    private float timer = 0f;
    [SerializeField] private float timePerSpawn = 3f;

    private GameObject player;

    [SerializeField] private float playerRange = 3f;

    private void Start()
    {
        player = GameObject.Find("Player");
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timePerSpawn)
        {
            timer -= timePerSpawn;
            canSpawn = true;
        }
        if (canSpawn && Vector2.Distance(transform.position, player.transform.position) < playerRange)
        {
            Spawn();
        }
    }


    private void Spawn()
    {
        GameObject actor = Instantiate(gob);
        actor.transform.position = transform.position;
        canSpawn = false;
    }
}
