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
        var hitPosition = transform.TransformPoint(hitCollider.offset);
        var hits = Physics2D.OverlapCircleAll(hitPosition, hitCollider.radius);

        foreach (var hit in hits)
        {
            if (!GameObject.Equals(hit.gameObject, gameObject))
            {
                var destructable = hit.gameObject.GetComponent<IDestructable>();

                if (destructable != null)
                {
                    destructable.Hit(damage);
                }
            }
        }
    }
}
