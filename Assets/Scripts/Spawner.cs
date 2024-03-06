using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject entity;
    GameObject spawnedEntity;
    float spawnTime = 10;

    void Awake()
    {
        SpawnEntity();
    }

    void SpawnEntity()
    {
        if (spawnedEntity == null) { 
            spawnedEntity = Instantiate(entity, transform.position, transform.rotation);
        }
        Invoke(nameof(SpawnEntity), spawnTime * Random.Range(1, 2));
    }
}
