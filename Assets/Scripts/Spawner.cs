using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject entity;
    GameObject spawnedEntity;
    float spawnDelayMax = 20;
    float spawnDelayMin = 10;

    void Awake()
    {
        SpawnEntity();
    }

    void SpawnEntity()
    {
        if (!spawnedEntity) { 
            spawnedEntity = Instantiate(entity, transform.position, transform.rotation);
        }
        Invoke(nameof(SpawnEntity), Random.Range(spawnDelayMin, spawnDelayMax));
    }
}
