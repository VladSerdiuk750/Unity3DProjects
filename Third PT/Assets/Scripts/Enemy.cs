using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    private NavMeshAgent _navMeshAgent;

    private bool seeTarget;

    [SerializeField]
    float shootDelay = 2f;

    [SerializeField]
    public Transform target { get; set; }

    [SerializeField]
    private int score = 25;

    [SerializeField]
    private GameObject explosionPrefab;

    void Awake()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 0.0f, shootDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            _navMeshAgent.SetDestination(target.position);
            CheckTargetVisibility();
        }

    }

    void Shoot()
    {
        if (seeTarget)
        {
            Vector3 targetDirection = target.position - Gun.transform.position;
            targetDirection.Normalize();

            ShootBullet(targetDirection);
        }
    }

    void CheckTargetVisibility()
    {
        Vector3 targetDirection = target.position - Gun.transform.position;

        Ray ray = new Ray(Gun.transform.position, targetDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == target)
            {
                seeTarget = true;
                return;
            }
        }

        seeTarget = false;
    }

    void Destroyed()
    {
        if (Random.Range(0, 100) < 50)
        {
            HealthBonus.Create(transform.position);
        }
        ScoreLabel.Score += score; 
        if(explosionPrefab != null)
        {
            Explosion.Create(transform.position, explosionPrefab);
        }
    }
}
