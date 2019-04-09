using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject rocketPrefab;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private Transform gun2;

    [SerializeField]
    private float shootPower = 5f;

    [SerializeField]
    private float bulletDamage = 10f;

    private float rocketDamage = 20f;

    [SerializeField]
    private float rocketDelay;

    private float rocketDelayCurrent;

    public Transform Gun { get => gun; set => gun = value; }
    public Transform Gun2 { get => gun2; set => gun2 = value; }

    public void ShootRocket(Vector3 direction)
    {
        if (rocketDelayCurrent <= 0)
        {
            rocketDelayCurrent = rocketDelay;
            GameObject newRocket = Instantiate(rocketPrefab, gun2.position, gun2.rotation) as GameObject;

            newRocket.GetComponent<Rigidbody>().AddForce(direction * shootPower);

            Damager rocketBehaviour = newRocket.GetComponent<Damager>();

            rocketBehaviour.Damage = rocketDamage;
            rocketBehaviour.Owner = gameObject;

            Destroy(newRocket.gameObject, 4);
        }   
    }

    protected void UpdateTimer()
    {
        if(rocketDelayCurrent > 0)
        {
            rocketDelayCurrent -= Time.deltaTime;
        }
    }

    public void ShootBullet(Vector3 direction)
    {
        GameObject newBullet = Instantiate(bulletPrefab, gun.position, gun.rotation) as GameObject;

        newBullet.GetComponent<Rigidbody>().AddForce(direction * shootPower);

        Damager bulletBehaviour = newBullet.GetComponent<Damager>();

        bulletBehaviour.Damage = bulletDamage;
        bulletBehaviour.Owner = gameObject;

        Destroy(newBullet.gameObject, 5);
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
