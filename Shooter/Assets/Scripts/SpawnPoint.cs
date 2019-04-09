using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    float spawnDelay = 3f;

    [SerializeField]
    Enemy enemyPrefab;
    
    private void Start()
    {
        Invoke("Spawn", Random.Range(spawnDelay - 1f, spawnDelay + 1f));
    }

    public void Spawn()
    {
        Enemy newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        newEnemy.Target = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke("Spawn", Random.Range(spawnDelay - 1f, spawnDelay + 1f));
    }
}
