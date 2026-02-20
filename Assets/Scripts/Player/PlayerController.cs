using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- Component References ---
    [Header("References")]
    [SerializeField] private ControlStats stats;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D feetCollider;

    private Rigidbody2D rb;

    // --- Movement Variables
    [Header("Movement")]
    private Vector2 moveVel;
    public bool facingRight;
    private Vector2 input;
    private bool running;

    // --- Jumping ---
    public float verticalVel { get; private set; }
    private bool jumping;
    private bool fastFalling;
    private bool falling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int jumpsUsed;

    // --- Jump Apex Variables ---
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    // --- Jump Buffer Variables ---
    private float jumpBufferTimer;
    private bool jumpReleaseDuringBuffer;

    // --- Coyote Time Variables ---
    private float coyoteTimer;

    // --- Grounded and Collision ---
    [Header("Grounded and Collision")]
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool grounded;
    private bool headBump;

    // --- Camera ---
    [SerializeField] private GameObject cameraFollow;
    private CameraFollowObject followObject;
    private float fallSpeedYDampingChangeThreshold;

    private void Awake()
    {
        facingRight = true;
        running = false;
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        headBump = false;

        followObject = cameraFollow.GetComponent<CameraFollowObject>();

        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
    }

    private void Update()
    {
        CountTimers();
        JumpCheck();

        // If we are falling past a certain speed threshold
        if (rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        // If we are standing still or moving up
        if (rb.velocity.y >= 0f && !CameraManager.instance.isLerpingYDamping && CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.lerpedFromPlayerFalling = false;

            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (grounded)
        {
            Move(stats.groundAcc, stats.groundDeAcc);
        } 
        else
        {
            Move(stats.airAcc, stats.airDeAcc);
        }
    }

    #region Move

    private void ListenForInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }

    private void Move(float acc, float deAcc)
    {
        ListenForInput();
        if (input != Vector2.zero)
        {
            TurnCheck();

            Vector2 targetVel = Vector2.zero;

            if (running)
            {
                targetVel = new Vector2(input.x, 0f) * stats.maxRunSpeed;
            }
            else
            {
                targetVel = new Vector2(input.x, 0f) * stats.maxWalkSpeed;
            }

            moveVel = Vector2.Lerp(moveVel, targetVel, acc * Time.fixedDeltaTime);
            rb.velocity = new Vector2(moveVel.x, rb.velocity.y);
        }
        else
        {
            moveVel = Vector2.Lerp(moveVel, Vector2.zero, deAcc * Time.fixedDeltaTime);
            rb.velocity = new Vector2(moveVel.x, rb.velocity.y);
        }
    }

    private void TurnCheck()
    {
        if (facingRight && input.x < 0)
        {
            Turn(false);
        }
        else if (!facingRight && input.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            facingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            facingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
        followObject.CallTurn();
    }
    #endregion

    #region Collision

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x * stats.headWidth, stats.headDetectRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, stats.headDetectRayLength, stats.groundLayer);

        if (headHit.collider != null)
        {
            headBump = true;
        }
        else
        {
            headBump = false;
        }
    }

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x, stats.groundDetectRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, stats.groundDetectRayLength, stats.groundLayer);

        if (groundHit.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }

    #endregion

    #region Jumping

    private void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = stats.jumpBufferTime;
            jumpReleaseDuringBuffer = false;
            Debug.Log(jumpsUsed);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (jumpBufferTimer > 0)
            {
                jumpReleaseDuringBuffer = true;
            }

            if (jumping && verticalVel > 0f)
            {
                if (isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    fastFalling = true;
                    fastFallTime = stats.timeForUpCancel;
                    verticalVel = 0f;
                }
                else
                {
                    fastFalling = true;
                    fastFallReleaseSpeed = verticalVel;
                }
            }
        }

        if (jumpBufferTimer > 0f && !jumping && (grounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (jumpReleaseDuringBuffer)
            {
                fastFalling = true;
                fastFallReleaseSpeed = verticalVel;
            }
        }
        else if (jumpBufferTimer > 0f && jumping && jumpsUsed < stats.numberOfJumps)
        {
            fastFalling = false;
            InitiateJump(1);
        }
        else if (jumpBufferTimer > 0f && falling && jumpsUsed < stats.numberOfJumps - 1)
        {
            InitiateJump(2);
            fastFalling = false;

        }

        if ((jumping || falling) && grounded && verticalVel <= 0f)
        {
            jumping = false;
            falling = false;
            fastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            jumpsUsed = 0;
            
            verticalVel = 0f;
        }
    }

    private void InitiateJump(int numJumpsUsed)
    {
        if (!jumping)
        {
            jumping = true;
        }

        jumpBufferTimer = 0f;
        jumpsUsed += numJumpsUsed;
        verticalVel = stats.initialJumpVel;
    }

    private void Jump()
    {
        if (jumping)
        {
            if (headBump)
            {
                fastFalling = true;
            }

            if (verticalVel >= 0f)
            {
                apexPoint = Mathf.InverseLerp(stats.initialJumpVel, 0f, verticalVel);

                if (apexPoint > stats.apexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < stats.apexHangTime)
                        {
                            verticalVel = 0f;
                        }
                        else
                        {
                            verticalVel = 0.01f;
                        }
                    }
                }
                else
                {
                    verticalVel += stats.gravity * Time.fixedDeltaTime;
                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }
            }
        }
        else if (!fastFalling)
        {
            verticalVel += stats.gravity * Time.fixedDeltaTime;
        }
        else if (verticalVel < 0f)
        {
            if (!falling)
            {
                falling = true;
            }
        }

        if (fastFalling)
        {
            if (fastFallTime >= stats.timeForUpCancel)
            {
                verticalVel += stats.gravity * Time.deltaTime;
            }
            else if (fastFallTime < stats.timeForUpCancel)
            {
                verticalVel = Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / stats.timeForUpCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }

        if (!grounded && !jumping)
        {
            if (!falling)
            {
                falling = true;
            }

            verticalVel += stats.gravity * Time.fixedDeltaTime;
        }
        verticalVel = Mathf.Clamp(verticalVel, -stats.maxFallSpeed, 50f);

        rb.velocity = new Vector2(rb.velocity.x, verticalVel);
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = stats.coyoteTime;
        }
    }

    #endregion
}
