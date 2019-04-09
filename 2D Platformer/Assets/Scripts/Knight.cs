using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Creature, IDestructable
{
    [SerializeField]
    private float stairSpeed = 2f;

    [SerializeField]
    private float jumpForce = 250f;

    [SerializeField]
    private bool onGround = true;

    [SerializeField]
    private float hitDelay = 0.4f;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private Transform groundCheck;

    private bool onStair;

    public bool OnStair
    {
        get => onStair;
        set
        {
            rigidbody.gravityScale = value ? 0 : 1;
            onStair = value;
        }
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        health = GameController.Instance.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = CheckGround();

        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        Vector2 velocity = rigidbody.velocity;

        velocity.x = Input.GetAxis("Horizontal") * speed;
        rigidbody.velocity = velocity;

        animator.SetBool("Jump", !onGround);

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            Invoke(nameof(Attack), hitDelay);
        }

        if (transform.localScale.x < 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = Vector3.one;
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (Input.GetButtonDown("Jump") && onGround)
        {
            rigidbody.AddForce(Vector2.up * jumpForce);
        }

        if (onStair)
        {
            velocity = rigidbody.velocity;
            velocity.y = Input.GetAxis("Vertical") * stairSpeed;
            rigidbody.velocity = velocity;
        }
    }

    private bool CheckGround()
    {
        var hits = Physics2D.LinecastAll(transform.position, groundCheck.position);
        foreach (var hit in hits)
        {
            if(!GameObject.Equals(hit.collider.gameObject, gameObject))
            {
                return true;
            }
        }
        return false;
    }

    private void Attack()
    {
        var hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (var hit in hits)
        {
            if(!GameObject.Equals(hit.gameObject, gameObject))
            {
                IDestructable destructable = hit.gameObject.GetComponent<IDestructable>();

                if(destructable != null)
                {
                    destructable.Hit(damage);
                    break;
                }
            }
        }
    }
}
