using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{
    float Health { get; set; }
    void Hit(float damage);
    void Die();
}
