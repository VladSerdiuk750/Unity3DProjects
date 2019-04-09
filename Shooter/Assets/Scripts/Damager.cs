using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
	public float Damage { get; set; }
	void OnCollisionEnter(Collision collision)
	{
		Destructable target = collision.gameObject.GetComponent<Destructable>();

		if (target != null)
		{
			target.Hit(Damage);
		}

		Destroy(gameObject);
	}
}
