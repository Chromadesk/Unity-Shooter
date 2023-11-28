using Interactable;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject shooter;
    public float damage;
    float initTime;

    private void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit == null || hit.CompareTag("Interactable") || hit.gameObject == shooter) return;

        //Hitting entities
        if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
        {
            hit.gameObject.GetComponent<EntityClass>().TakeDamage(damage);
        }

        //Hitting cover
        if (hit.CompareTag("Cover"))
        {
            if (!hit.GetComponent<Cover>().isSolid) return;
        }

        Destroy(gameObject);
    }

    void Awake()
    {
        initTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - initTime >= 8) Destroy(gameObject);
    }
}