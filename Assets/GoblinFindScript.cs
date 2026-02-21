using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GoblinFindScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager gameManager;
    public Transform closistEnemylocation = null; 
    public float minDistance = Mathf.Infinity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (gameManager.enemyList.Count > 0)
        //{
        //    //getClosestEnemy();
            

            
        //    //Debug.Log();
        //}
        
    }

    public Transform getClosestEnemy(List<GameObject> enemyList)
    {
        for(int i =0; i < enemyList.Count; i++)
        {
            Vector3 position = transform.position;
            //Debug.Log(position);
            //Debug.Log(enemyLocation.transform.position);
            float distance = Vector3.Distance(enemyList[i].transform.position, position);
            //Debug.Log(distance);
            //Debug.Log($"Possible closer enemy {enemyLocation.position.x}and {enemyLocation.position.y}");
            if (distance < minDistance)
            {

                Debug.Log($"New closest Enemy at {enemyList[i].transform.position.x} and {enemyList[i].transform.position.y}");
                closistEnemylocation = enemyList[i].transform;

                minDistance = distance;
            }

        }

        return closistEnemylocation;
    }
}

