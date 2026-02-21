using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> enemyList = new List<GameObject>();
    public List<SpawnEnemy> spawnerList = new List<SpawnEnemy>();

    public GoblinFindScript goblinFind;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FindEnemies()
    {
        goblinFind.getClosestEnemy(enemyList);
    }

    //public WaitUntil RotateSpawner()
    //{
    //    for(int i = 0; i < spawnerList.Count; i++)
    //    {
    //        Debug.Log($"Timer starting for {i}");
    //        spawnerList[i].startTimer = true;
    //        yield return new WaitUntil(() => spawnerList[i].isDone == true);
    //        Debug.Log($"Timer done for {i}");
    //        spawnerList[i].isDone = false;
    //    }
    //   yield return new WaitUntil();
    //}
}
