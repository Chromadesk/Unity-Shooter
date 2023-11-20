using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit == null) return;
        if (hit.CompareTag("Player")) Debug.Log("farted on");
        Destroy(gameObject);
    }
}