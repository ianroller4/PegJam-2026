using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> enemyList = new List<GameObject>();
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
}
