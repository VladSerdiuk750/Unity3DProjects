using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;

    public bool rotateBar = true;

    public Destructable owner;

    private void Update()
    {
        healthBar.fillAmount = owner.hitPoints / owner.hitPointsMax;
        healthBar.color = Color.blue;

        if (rotateBar == true)
        {
            transform.forward = Camera.main.transform.position - transform.position;
        }
    }
}
