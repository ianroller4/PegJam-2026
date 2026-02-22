using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boomBoom : MonoBehaviour
{
    [SerializeField] private float damage = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject hitObject = collision.gameObject;

            if (hitObject.GetComponent<Health>() != null)
            {
                hitObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    private void Start()
    {
        Destroy(gameObject, 0.1f);
    }
}
