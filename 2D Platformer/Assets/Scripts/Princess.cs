using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Knight knight = collider.gameObject.GetComponent<Knight>();

        if (knight != null)
        {
            GameController.Instance.PrincessFound();
        }
    }
}
