using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberGob : Gob
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    [SerializeField] private float range = 2f;

    // --- Target ---
    private GameObject target;

    public GobManager gobManager;
    private EnemyManager enemyManager;

    [SerializeField] private GameObject boom;

    private Animator animator;

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

        animator = GetComponent<Animator>();    
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
            Vector3 myPosition = transform.position;
            if (Vector2.Distance(enemyManager.enemies[i].transform.position, myPosition) <= range)
            {
                target = enemyManager.enemies[i].gameObject;
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
        transform.position = player.transform.position + 2.5f * Vector3.up;
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
    }

    public override void EnterAttack()
    {
        state = GobState.ATTACK;
        animator.SetBool("explode", true);
        Death();
    }

    public void Death()
    {
        gobManager.RemoveGob(this);
        Destroy(gameObject, 1.1f);
    }
}
