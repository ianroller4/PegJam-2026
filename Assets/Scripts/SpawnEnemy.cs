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

    public int maxEnemies = 3;
    public bool startTimer = false;
    public float timer = 0;
    public float maxTime;
    public bool isDone = false;


    void Start()
    {
        startTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            if(timer >= maxTime)
            {
                Debug.Log(timer);
                startTimer = false;
                timer = 0;
                SpawnEnemies();
                
            }
        }
    }

    public void SpawnEnemies()
    {
        for(int i = 0; i < maxEnemies; i++)
        {
            float randomX = Random.Range(-5, 5);
            float randomY = Random.Range(0, 5);

            Vector3 randomSpawnPoint = new Vector3(randomX, randomY, 0);

            GameObject instantiateEnemy = Instantiate(enemy);
            instantiateEnemy.transform.position = randomSpawnPoint;

            gameManager.enemyList.Add(instantiateEnemy);
            gameManager.FindEnemies();
        }
    }
}
