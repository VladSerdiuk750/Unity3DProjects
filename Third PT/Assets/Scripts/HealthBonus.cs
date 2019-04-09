using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destructable otherHealth = other.gameObject.GetComponent<Destructable>();

        if (otherHealth != null)
        {
            otherHealth.HitPointsCurrent = otherHealth.HitPoints;

            Destroy(gameObject);
        }
    }

    public static void Create(Vector3 position)
    {
        Instantiate(Resources.Load<GameObject>("Health"), position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
