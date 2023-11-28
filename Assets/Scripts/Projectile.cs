using Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject shooter;

    private void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit == null || hit.CompareTag("Interactable") || hit.gameObject == shooter) return;

        //Hitting entities
        if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
        {

        }

        //Hitting cover
        if (hit.CompareTag("Cover"))
        {
            if (!hit.GetComponent<Cover>().isSolid) return;
        }

        Destroy(gameObject);
    }
}