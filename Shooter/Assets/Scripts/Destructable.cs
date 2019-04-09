using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
	public float hitPointsMax = 100f;

	public float hitPoints;

	// Start is called before the first frame update
	void Start()
	{
		hitPoints = hitPointsMax;
	}

	public void Hit(float damage)
	{
		hitPoints -= damage;

		Debug.Log($"hp left: {hitPoints}");

		if (hitPoints <= 0f)
		{
			Die();
		}
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}
