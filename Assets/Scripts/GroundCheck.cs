using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;


    public bool GetIsGrounded() { return isGrounded;}
    private void OnTriggerEnter2D(Collider2D col)
    {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        isGrounded = false;
    }
}