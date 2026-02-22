using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float currHP;
    [SerializeField] private float MAX_HP = 3f;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        currHP = MAX_HP;
        if (player == null )
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        else
        {
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }
    }

    public void TakeDamage(float damage)
    {
        currHP -= damage;

        if (currHP > 0)
        {
            // Damage Flicker
            StartCoroutine(DamageFlicker());
        }

        if (currHP <= 0)
        {
            DeathFromDamage();
        }
    }

    private IEnumerator DamageFlicker()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;
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
            SceneManager.LoadScene(2);
        }
    }
}
