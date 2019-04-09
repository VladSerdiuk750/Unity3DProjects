using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    float spawnDelay = 3f;

    [SerializeField]
    Enemy[] enemyPrefabs;


    public void Spawn()
    {
        Enemy newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position, transform.rotation) as Enemy;
        newEnemy.target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        InvokeRepeating("Spawn", 0.0f, spawnDelay);
    }
}
