using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var knight = collider.gameObject.GetComponent<Knight>();

        if(knight != null)
        {
            knight.OnStair = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collider)
    {
        var knight = collider.gameObject.GetComponent<Knight>();

        if (knight != null)
        {
            knight.OnStair = false;
        }
    }
}
