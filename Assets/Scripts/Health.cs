using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float currHP;
    [SerializeField] private float MAX_HP = 3f;

    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        currHP = MAX_HP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currHP -= damage;

        if (currHP > 0)
        {
            // Damage Flicker
        }

        if (currHP <= 0)
        {
            DeathFromDamage();
        }
    }

    public void DeathFromDamage()
    {
        Destroy(gameObject);
    }
}
