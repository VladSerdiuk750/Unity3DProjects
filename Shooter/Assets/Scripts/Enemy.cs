using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Transform[] wayPoints;

    private int currentWayPoint;

    private float minDistance = 2f;

    public Transform Target { get; set; }


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (wayPoints.Length > 0)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;

            if ((player.position - transform.position).magnitude < 10)
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                if (Target == null || (transform.position - Target.position).magnitude <= minDistance)
                {
                    Target = wayPoints[currentWayPoint];
                    currentWayPoint++;
                    if (currentWayPoint >= wayPoints.Length)
                    {
                        currentWayPoint = 0;
                    }
                }
                navMeshAgent.SetDestination(Target.position);
            }
        }
        else
        {
            if (Target != null)
            {
                navMeshAgent.SetDestination(Target.position);
            }
        }
    }
}
