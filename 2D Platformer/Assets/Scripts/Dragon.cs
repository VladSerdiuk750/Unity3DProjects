using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Creature
{   
    public float Speed { get => speed; set => speed = value; }

    [SerializeField]
    private CircleCollider2D hitCollider;

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        var velocity = rigidbody.velocity;
        velocity.x = speed * transform.localScale.x * -1;
        rigidbody.velocity = velocity;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        var knight = collider.gameObject.GetComponent<Knight>();

        if(knight != null)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        transform.localScale = transform.localScale.x < 0 ? Vector3.one : new Vector3(-1, 1, 1);
    }

    public void Attack()
    {
        Vector3 hitPosition = transform.TransformPoint(hitCollider.offset);
        DoHit(hitPosition, hitCollider.radius, Damage);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Knight>() != null)
        {
            for (int i = 0; i < other.contacts.Length; i++)
            {
                Vector2 fromDragonToContactVector = other.contacts[i].point - (Vector2)transform.position;
                if (Vector2.Angle(fromDragonToContactVector, Vector2.up) < 45)
                {
                    Die();
                }
            }
        }
    }
}
