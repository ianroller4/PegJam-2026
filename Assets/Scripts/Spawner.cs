using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private float timer = 0f;
    [SerializeField] private float timePerSpawn = 3f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timePerSpawn)
        {
            timer -= timePerSpawn;
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject actor = Instantiate(prefab);
        actor.transform.position = transform.position;
    }
}
