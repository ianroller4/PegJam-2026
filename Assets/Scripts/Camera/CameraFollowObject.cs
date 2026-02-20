using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRotationTime = 0.05f;

    private Coroutine turnCoroutine;

    private PlayerController player;

    private bool facingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<PlayerController>();
        facingRight = player.facingRight;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();

        float yRot = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRot = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRot, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        facingRight = !facingRight;

        if (facingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
