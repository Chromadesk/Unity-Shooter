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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit == null || hit.CompareTag("Interactable") || hit.gameObject == shooter) return;

        //Hitting entities
        if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
        {
            hit.GetComponent<EntityClass>().TakeDamage(damage);
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