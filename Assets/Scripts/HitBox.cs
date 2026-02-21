using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

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
}
