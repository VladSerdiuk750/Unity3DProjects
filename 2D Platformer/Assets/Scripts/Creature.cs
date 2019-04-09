using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IDestructable
{
    protected float health = 100f;

    public float Health
    {
        get => health;
        set => health = value;
    }

    protected Animator animator;

    protected Rigidbody2D rigidbody;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float damage;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Hit(float damage)
    {
        Health -= damage;
        GameController.Instance.Hit(this);
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
