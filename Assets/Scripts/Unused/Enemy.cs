using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public float jumpForce = 6f;

    private Rigidbody2D rb;

    private float moveTimer = 0f;
    private float jumpTimer = 0f;
    private float nextMoveTimer = 2f;
    private float nextJumpTimer = 2f;


    [SerializeField]
    private GroundCheck groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        jumpTimer += Time.deltaTime;

        Move();
    }

    void FixedUpdate()
    {

    }

    public void Move()
    {
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
            if (jumpTimer >= nextJumpTimer)
            {
                Jump();
                jumpTimer = 0f;
                nextJumpTimer = Random.Range(1f, 3f);
            }

            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
