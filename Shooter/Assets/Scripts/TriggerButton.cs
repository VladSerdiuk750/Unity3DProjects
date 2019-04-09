using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    public GameObject targetGameObject;
   
    void OnTriggerEnter(Collider other)
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(true);
        }
    }
}
