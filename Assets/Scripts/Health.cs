using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private float currHP;
    [SerializeField] private float MAX_HP = 3f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        currHP = MAX_HP;
        if (spriteRenderer == null )
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
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
        if (player == null)
        {
            if (GetComponent<EnemyNew>() != null)
            {
                GetComponent<EnemyNew>().enemyManager.RemoveEnemy(GetComponent<EnemyNew>());
            }
            else if (GetComponent<DaggerGob>() != null)
            {
                GetComponent<DaggerGob>().gobManager.RemoveGob(GetComponent<DaggerGob>());
            }
            else if (GetComponent<MagicGob>() != null)
            {
                GetComponent<MagicGob>().gobManager.RemoveGob(GetComponent<MagicGob>());
            }
            else if (GetComponent<TankGob>() != null)
            {
                GetComponent<TankGob>().gobManager.RemoveGob(GetComponent<TankGob>());
            }
            Destroy(gameObject);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
