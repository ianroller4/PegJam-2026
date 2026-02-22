using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNew : MonoBehaviour
{
    private enum State
    {
        PATROL,
        ATTACK
    }
    private State state;

    private Rigidbody2D rb;

    [SerializeField] private GameObject hitBox;

    // --- Movement Timers ---
    private float moveTimer = 0f;
    private float moveTimerMax = 2f;

    // --- Attack Timers ---
    private float attackTimer = 0f;
    [SerializeField] private float attackTimerMax = 1f;
    [SerializeField] private float range = 1f;

    public bool facingRight = true;

    // --- Target ---
    private GameObject target;

    // --- Move Variables ---
    [SerializeField] private float moveSpeed = 2f;
    private Vector3 direction = Vector3.right;

    public EnemyManager enemyManager;
    private GobManager gobManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        EnterPatrol();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        gobManager = GameObject.FindObjectOfType<GobManager>();

        enemyManager.AddEnemy(this);

        if (Random.Range(0f, 1f) < 0.5)
        {
            direction = Vector3.left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.PATROL:
                UpdatePatrol();
                break;
            case State.ATTACK:
                UpdateAttack();
                break;
        }
    }

    private void EnterPatrol()
    {
        state = State.PATROL;
    }

    private void UpdatePatrol()
    {
        if (Vector3.Distance(gobManager.player.transform.position, transform.position) < range)
        {
            target = gobManager.player;
            EnterAttack();
        }

        if (target == null)
        {
            for (int i = 0; i < gobManager.gobs.Count; i++)
            {
                Vector3 gobPos = gobManager.gobs[i].transform.position;
                if (gobManager.gobs[i].GetComponent<BomberGob>() != null)
                {
                    gobPos -= new Vector3(0f, -0.5f, 0f);
                }
                if (Vector3.Distance(gobPos, transform.position) < range)
                {
                    target = gobManager.gobs[i].gameObject;
                    EnterAttack();
                    break;
                }
            }
        }
        Move();
    }

    private void Move()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveTimerMax)
        {
            moveTimer -= moveTimerMax;
            if (Random.Range(0f, 1f) < 0.5)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }

        if (!facingRight && direction == Vector3.right)
        {
            Flip();
        }
        else if (facingRight && direction == Vector3.left)
        {
            Flip();
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void EnterAttack()
    {
        state = State.ATTACK;
    }

    private void UpdateAttack()
    {
        if (target == null || Vector2.Distance(target.transform.position, transform.position) > range)
        {
            EnterPatrol();
        }
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTimerMax)
        {
            attackTimer -= attackTimerMax;
            Attack();
        }
        if (target == null)
        {
            EnterPatrol();
        }
    }

    private void Attack()
    {
        GameObject hB = Instantiate(hitBox);
        if (facingRight)
        {
            hB.transform.position = transform.position + Vector3.right;
        }
        else
        {
            hB.transform.position = transform.position + Vector3.left;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void Death()
    {
        enemyManager.RemoveEnemy(this);
    }
}
