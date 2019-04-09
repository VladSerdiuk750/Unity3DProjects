using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public float Damage { get; set; }

    public GameObject Owner { get { return owner; } set { owner = value; } }

    [SerializeField]
    private GameObject explosionPrefab;

    private GameObject owner;

    private float radius;

    void OnCollisionEnter(Collision collision)
    {
        if (!GameObject.Equals(collision.gameObject, owner))
        {
            if(radius > 0)
            {
                CauseExplosionDamage();
            }
            else
            {
                Destructable target = collision.gameObject.GetComponent<Destructable>();

                if (target != null)
                {
                    target.Hit(Damage);
                }
            }

            if (explosionPrefab != null)
            {
                Explosion.Create(transform.position, explosionPrefab);
            }

            ParticleSystem trail = gameObject.GetComponent<ParticleSystem>();
            if (trail != null)
            {
                Destroy(trail.gameObject, trail.startLifetime);
                trail.Stop();
                trail.transform.SetParent(null);
            }
            Destroy(gameObject);
        }
    }

    private void CauseExplosionDamage()
    {
        Collider[] explosionVictims = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < explosionVictims.Length; i++)
        {
            Vector3 vectorToVictim = explosionVictims[i].transform.position - transform.position;
            float decay = 1 - (vectorToVictim.magnitude / radius);
            Destructable currentVictim = gameObject.GetComponent<Destructable>();
            if(currentVictim != null)
            {
                currentVictim.Hit(Damage * decay);
            }
            Rigidbody victimRigidbody = explosionVictims[i].gameObject.GetComponent<Rigidbody>();
            if(victimRigidbody != null)
            {
                victimRigidbody.AddForce(vectorToVictim.normalized * decay * 1000);
            }
        }


    }
}
