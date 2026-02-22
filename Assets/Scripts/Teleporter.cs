using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject go = collision.gameObject;
            if (go.name == "Body")
            {
                go = go.transform.parent.transform.parent.gameObject;
            }
            if (go != null)
            {
                go.transform.position = target;
            }
        }
    }
}
