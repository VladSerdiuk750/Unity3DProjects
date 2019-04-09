using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    float hitPoints;

    [SerializeField]
    float hitPointsCurrent;

    public float HitPoints { get => hitPoints; set => hitPoints = value; }
    public float HitPointsCurrent { get => hitPointsCurrent; set => hitPointsCurrent = value; }



    // Start is called before the first frame update
    void Start()
    {
        HitPointsCurrent = HitPoints;
    }

    public void Hit(float damage)
    {
        HitPointsCurrent -= damage;

        if (HitPointsCurrent < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        BroadcastMessage("Destroyed");
    }
}
