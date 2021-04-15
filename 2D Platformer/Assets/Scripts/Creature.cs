using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

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

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField]
    protected float damage;

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void ReceiveHit(float damage)
    {
        Health -= damage;
        GameController.Instance.Hit(this);
        if (Health <= 0)
        {
            Die();
        }
    }

    protected void DoHit(Vector3 hitPosition, float hitRadius, float hitDamage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPosition, hitRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!GameObject.Equals(hits[i].gameObject, gameObject))
            {
                IDestructable destructable = hits[i].gameObject.GetComponent<IDestructable>();
                if (destructable != null)
                {
                    destructable.ReceiveHit(hitDamage);
                }
            }
        }
    }

    public void Die()
    {
        GameController.Instance.Killed(this);
    }


}
