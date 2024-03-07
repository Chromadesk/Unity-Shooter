using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject entity;
    GameObject spawnedEntity;
    float spawnDelayMax = 2;
    float spawnDelayMin = 1;

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
