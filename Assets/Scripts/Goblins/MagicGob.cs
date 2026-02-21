using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGob : Gob
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    [SerializeField] private GameObject projectile;

    private float attackTimer = 0f;
    [SerializeField] private float attackTimerMax = 1f;

    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
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
            case GobState.DEATH:
                UpdateDeath();
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
    }

    public override void EnterAttack()
    {
        state = GobState.ATTACK;
    }

    public override void UpdateAttack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTimerMax)
        {
            attackTimer -= attackTimerMax;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject proj = Instantiate(projectile);
        proj.GetComponent<Projectile>().right = facingRight;
        if (facingRight)
        {
            proj.transform.position = transform.position + Vector3.right;
        }
        else
        {
            proj.transform.position = transform.position + Vector3.left;
        }

    }

    public override void EnterDeath()
    {
        state = GobState.DEATH;
    }

    public override void UpdateDeath()
    {

    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
