using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Control Stats")]
public class ControlStats : ScriptableObject
{
    [Header("Movement")]
    [SerializeField, Range(1f, 100f)] public float maxWalkSpeed = 12.5f;
    [SerializeField, Range(0.25f, 50f)] public float maxRunSpeed = 20f;
    [SerializeField, Range(0.25f, 50f)] public float groundAcc = 5f;
    [SerializeField, Range(0.25f, 50f)] public float groundDeAcc = 20f;
    [SerializeField, Range(0.25f, 50f)] public float airAcc = 5f;
    [SerializeField, Range(0.25f, 50f)] public float airDeAcc = 5f;

    [Header("Grounded & Collision")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float groundDetectRayLength = 0.2f;
    [SerializeField] public float headDetectRayLength = 0.2f;
    [SerializeField, Range(0f, 1f)] public float headWidth = 0.75f;

    [Header("Jump")]
    [SerializeField] public float jumpHeight = 6.5f;
    [SerializeField, Range(1f, 1.1f)] public float jumpHeightCompensationFactor = 1.05f;
    [SerializeField] public float timeTillApex = 0.35f;
    [SerializeField, Range(0.01f, 5f)] public float gravityOnReleaseMult = 2f;
    [SerializeField] public float maxFallSpeed = 26f;
    [SerializeField] public int numberOfJumps = 2;

    [Header("Jump Cut")]
    [SerializeField, Range(0.02f, 0.03f)] public float timeForUpCancel = 0.027f;

    [Header("Jump Apex")]
    [SerializeField, Range(0.5f, 1f)] public float apexThreshold = 0.97f;
    [SerializeField, Range(0.01f, 1f)] public float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [SerializeField, Range(0f, 1f)] public float jumpBufferTime = 0.125f;

    [Header("Coyote Time")]
    [SerializeField, Range(0f, 1f)] public float coyoteTime = 0.1f;

    public float gravity { get; private set; }
    public float initialJumpVel { get; private set; }
    public float adjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        adjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
        gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(timeTillApex, 2f);
        initialJumpVel = Mathf.Abs(gravity) * timeTillApex;
    }
}
