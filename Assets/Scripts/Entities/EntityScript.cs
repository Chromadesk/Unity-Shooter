using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactable;

namespace Scripts
{
    public abstract class EntityClass : MonoBehaviour
    {
        [SerializeField] protected string displayName;
        [SerializeField] protected float health = 100;
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected GameObject projectile;
        [SerializeField] protected float projectileVelocity;
        protected Rigidbody rB;
        GameObject touchedInteractable;
        public bool isStanding = true;
        public Cover cover = null;

        void Start()
        {
            rB = GetComponent<Rigidbody>();
        }

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public float Health { get => health; set { health = Mathf.Round(value); if (health <= 0) OnDied(); } }

        protected abstract void OnDied();

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }

        protected void FireProjectile()
        {
            if (!isStanding) return;
            Vector3 pos = transform.position;
            GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * projectileVelocity, ForceMode.VelocityChange);
            bullet.GetComponent<Projectile>().shooter = gameObject;
        }

        //If standing in an Interactable, will activate that trigger's function.
        //I.E: If standing in the Interactable of cover, signals that cover to enter it.
        protected void Interact()
        {
            if (touchedInteractable && touchedInteractable.CompareTag("Interactable")) 
            {
                GameObject interactable = touchedInteractable.transform.parent.gameObject;
                interactable.GetComponent<InteractableClass>().Use(gameObject, touchedInteractable);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        { 
            if (other.isTrigger && other.gameObject.CompareTag("Interactable")) touchedInteractable = other.gameObject; 
        }
        private void OnTriggerExit(Collider other) { touchedInteractable = null; }

    }
}
