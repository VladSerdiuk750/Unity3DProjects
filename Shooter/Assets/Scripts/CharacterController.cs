using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public Rigidbody Rigidbody;

	public int speed = 20;

	public int jumpSpeed = 500;

	public int maxSlope = 45;

	public bool onGround;

	public GameObject bulletPrefab;

	public GameObject Gun;

	public float BulletDamage;

	public int FirePower = 10000;

    // Update is called once per frame
    void Update()
    {
		if (onGround)
		{
			Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
			movement.Normalize();
			Rigidbody.AddForce(movement * speed);

			LookAtTarget();

			if (Input.GetKeyDown(KeyCode.Space))
			{
				Rigidbody.AddForce(transform.up * jumpSpeed);
			}

			if (Input.GetButtonDown("Fire1"))
			{
				GameObject bullet = Instantiate(bulletPrefab, Gun.transform.position, Gun.transform.rotation);

				bullet.GetComponent<Rigidbody>().AddForce(Gun.transform.forward * FirePower);

				bullet.GetComponent<Damager>().Damage = BulletDamage;

				Destroy(bullet, 5);
			}
		}
		//Rigidbody.AddForce(Vector3.left * Input.GetAxis("Horizontal") * 20);
		//Rigidbody.AddForce(Vector3.back * Input.GetAxis("Vertical") * 20);
	}

	private void LookAtTarget()
	{
		Plane plane = new Plane(Vector3.up, transform.position);

		//RaycastHit hit;

		float distance;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out distance))
		{
			Vector3 position = ray.GetPoint(distance);
			transform.LookAt(position);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			if (collision.contacts[i].otherCollider.tag == "Ground")
			{
				if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)
				{
					onGround = true;
					break;
				}
			}
		}
	}

	void OnCollisionStay(Collision collision)
	{
		//Debug.Log("Stay");
	}

	void OnCollisionExit(Collision collision)
	{
		for (int i = 0; i < collision.contacts.Length; i++)
		{
			if (collision.rigidbody.tag == "Ground")
			{
				onGround = false;
				break;
			}
		}
	}
}
