using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float damage = 2f;
    
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
        if (collision != null)
        {
            GameObject hitObject = collision.gameObject;

            if (hitObject.GetComponent<Health>() != null)
            {
                hitObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
        Destroy(gameObject, 0.2f);
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
