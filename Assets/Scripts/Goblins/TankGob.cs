using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGob : Gob
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    [SerializeField] private GameObject hitBox;

    private float attackTimer = 0f;
    [SerializeField] private float attackTimerMax = 1f;

    public bool facingRight = true;

    [SerializeField] private float range = 1f;

    // --- Target ---
    private GameObject target;

    public GobManager gobManager;
    private EnemyManager enemyManager;

    private AudioSource audioSource;

    [SerializeField] private AudioClip wee;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        gobManager = GameObject.FindObjectOfType<GobManager>();

        gobManager.AddGob(this);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GobState.IDLE:
                UpdateIdle();
                break;
            case GobState.HELD:
                UpdateHeld();
                break;
            case GobState.THROWN:
                break;
            case GobState.ATTACK:
                UpdateAttack();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject go = collision.gameObject;
            if (go.transform.parent != null)
            {
                GameObject goParent = go.transform.parent.gameObject;
                if (goParent.transform.parent != null)
                {
                    GameObject goGrandParent = goParent.transform.parent.gameObject;
                    if (goGrandParent.GetComponent<Sack>() != null)
                    {
                        goGrandParent.GetComponent<Sack>().AddGob(this);
                        circleCollider.enabled = false;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnterIdle();
        rb.velocity = Vector3.zero;
    }

    public override void EnterIdle()
    {
        state = GobState.IDLE;
        circleCollider.enabled = true;
    }

    public override void UpdateIdle()
    {
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (Vector2.Distance(enemyManager.enemies[i].transform.position, transform.position) <= range)
            {
                target = enemyManager.enemies[i].gameObject;
                if (target.transform.position.x < transform.position.x)
                {
                    if (facingRight)
                    {
                        Flip();
                    }
                }
                if (target.transform.position.x > transform.position.x)
                {
                    if (!facingRight)
                    {
                        Flip();
                    }
                }
                EnterAttack();
                break;
            }
        }
    }

    public override void EnterHeld()
    {
        state = GobState.HELD;
        rb.simulated = false;
        boxCollider.enabled = false;
    }

    public override void UpdateHeld()
    {
        transform.position = player.transform.position + Vector3.up;
    }

    public override void EnterThrown(bool right)
    {
        state = GobState.THROWN;
        rb.simulated = true;
        boxCollider.enabled = true;
        Vector2 force;
        if (right)
        {
            force = new Vector2(1, 1);
        }
        else
        {
            force = new Vector2(-1, 1);
        }

        force = force.normalized * 500f;

        rb.AddForce(force);
        audioSource.clip = wee;
        audioSource.Play();
    }

    public override void EnterAttack()
    {
        state = GobState.ATTACK;
    }

    public override void UpdateAttack()
    {
        if (target == null || Vector2.Distance(target.transform.position, transform.position) > range)
        {
            EnterIdle();
        }
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTimerMax)
        {
            attackTimer -= attackTimerMax;
            Attack();
        }
        if (target == null)
        {
            EnterIdle();
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

    public void Death()
    {
        gobManager.RemoveGob(this);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
