using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public GoblinFindScript goblin;
    public GameManager gameManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies()
    {
        float randomX = Random.Range(-10, 10);
        float randomY = Random.Range(0, 10);
        
        Vector3 randomSpawnPoint = new Vector3(randomX, randomY, 0);

        GameObject instantiateEnemy = Instantiate(enemy);
        instantiateEnemy.transform.position = randomSpawnPoint;
        
      

        gameManager.enemyList.Add(enemy);
        gameManager.FindEnemies();
    }

}
