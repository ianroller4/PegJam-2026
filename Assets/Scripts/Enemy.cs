using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }
    private EnemyState currentState;

    private Rigidbody2D rb;

    private Vector3 attackPos;

    // timer
    private float moveTimer = 0f;
    private float jumpTimer = 0f;
    private float nextMoveTimer = 2f;
    private float nextJumpTimer = 2f;
    private float targetUpdateTimer = 0f;
    private float attackTimer = 0f;


    // enemy settings
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float chaseSpeed = 4f;
    [SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    private float detectRadius = 3f;
    [SerializeField]
    private float chaseStopDistance = 4f;
    [SerializeField]
    private float attackRange = 2f;
    [SerializeField]
    private float targetUpdateInterval = 1f;

    // references
    [SerializeField]
    private GroundCheck groundCheck;

    [SerializeField]
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Move();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }

        Debug.Log(currentState);
    }

    public void Move()
    {
        moveTimer += Time.deltaTime;
        //jumpTimer += Time.deltaTime;

        if (groundCheck.GetIsGrounded())
        {
            // Randomize direction and speed
            if (moveTimer >= nextMoveTimer)
            {
                int[] speeds = { -4, -3, -2, 2, 3, 4 };
                speed = speeds[Random.Range(0, speeds.Length)];
                moveTimer = 0f;
                nextMoveTimer = Random.Range(1f, 3f);
            }

            // Jump
            //if (jumpTimer >= nextJumpTimer)
            //{
            //    Jump();
            //    jumpTimer = 0f;
            //    nextJumpTimer = Random.Range(1f, 3f);
            //}
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);

        GameObject searchingTarget = searchTarget();

        if (searchingTarget != null)
        {
            Debug.Log("Closest Target: " + searchingTarget.name);
            target = searchingTarget;
            currentState = EnemyState.Chase;
        }

    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    private GameObject searchTarget()
    {
        Vector2 currentPos = transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("Player");

        Collider2D[] cols = Physics2D.OverlapCircleAll(currentPos, detectRadius, layerMask);
        Debug.Log(LayerMask.NameToLayer("Player"));
        GameObject closest = null;
        float minDist = 999f;

        for (int i = 0; i < cols.Length; i++)
        {
            Collider2D col = cols[i];

            float dist = Vector2.Distance(currentPos, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.gameObject;
            }
        }

        return closest;
    }

    private void Chase()
    {
        if (target == null)
        {
            currentState = EnemyState.Idle;
            return;
        }
        float dist = Vector2.Distance(transform.position, target.transform.position);

        if (groundCheck.GetIsGrounded() && dist < attackRange)
        {
            currentState = EnemyState.Attack;
            return;
        }

        if (dist > chaseStopDistance)
        {
            target = null;
            currentState = EnemyState.Idle;
            return;
        }

        targetUpdateTimer += Time.deltaTime;

        float dx = target.transform.position.x - transform.position.x;
        float dir = Mathf.Sign(dx);
        rb.velocity = new Vector2(dir * Mathf.Abs(chaseSpeed), rb.velocity.y);

        if (targetUpdateTimer >= targetUpdateInterval)
        {
            targetUpdateTimer = 0f;

            GameObject searchingTarget = searchTarget();

            if (searchingTarget != null)
            {
                Debug.Log("Closest Target: " + searchingTarget.name);
                target = searchingTarget;
            }
        }
    }

    private void Attack()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);

        if (target == null)
        {
            attackTimer = 0f;
            currentState = EnemyState.Idle;
            return;
        }

        if (attackTimer == 0f && target != null)
        {
            attackPos = target.transform.position; // saves the attack position
            Vector3 dir = (target.transform.position - transform.position).normalized;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= 1.1f) // attack time 1.1secs
        {
            attackTimer = 0f;
            target = null;
            currentState = EnemyState.Idle;
        }
    }
}
