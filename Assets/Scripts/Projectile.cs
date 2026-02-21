using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Animator animator;

    public bool right = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!right)
        {
            transform.Rotate(0f, 180f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("hit", true);
        // deal damage
        Destroy(this.gameObject, 0.2f);
    }

    private void Move()
    {
        if (right)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        
    }

    private void Flip()
    {
        if (right)
        {

        }
        else
        {

        }
    }
}
