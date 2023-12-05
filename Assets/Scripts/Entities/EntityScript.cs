using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactable;
using UnityEditorInternal;

namespace Scripts
{
    public abstract class EntityClass : MonoBehaviour
    {
        [SerializeField] protected string displayName;
        [SerializeField] protected float health = 100;
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float attackDamage = 40f;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected GameObject projectile;
        [SerializeField] protected GameObject gunSmokeParticle;
        [SerializeField] protected float projectileVelocity;
        [SerializeField] AudioSource fireSound;

        GameObject touchedInteractable;

        protected Rigidbody rB;
        protected bool hasAttacked = false;

        public bool isStanding = true;
        public Cover cover = null;

        void Start()
        {
            rB = GetComponent<Rigidbody>();
        }

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public float Health { get => health; set { health = Mathf.Round(value); if (health <= 0) OnDied(); OnHealthSet(health); } }

        protected abstract void OnDied();
        protected virtual void OnHealthSet(float value) { }

        bool damageDebounce = false;
        public void TakeDamage(float damage)
        {
            if (damageDebounce) return;
            Health -= damage;
            damageDebounce = true;
            Invoke(nameof(resetDamageDebounce), 0.05f);
        }
        void resetDamageDebounce() { damageDebounce = false; }

        protected void FireProjectile()
        {
            if (!isStanding || hasAttacked) return;
            
            //If the shooter is too close to a wall, don't spawn a bullet.
            if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, projectile.transform.localScale.z))
            {
                Vector3 pos = transform.position;
                GameObject bullet = Instantiate(projectile, transform.forward * 1f + transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * projectileVelocity, ForceMode.VelocityChange);
                bullet.GetComponent<Projectile>().shooter = gameObject;
                bullet.GetComponent<Projectile>().damage = attackDamage;
            }

            //Bullet effects
            GameObject smoke = Instantiate(gunSmokeParticle, transform);
            smoke.GetComponent<ParticleSystem>().Play();
            fireSound.Play();

            //Start cooldown
            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        void ResetAttack() { hasAttacked = false; }

        //If standing in an Interactable, will activate that trigger's function.
        //I.E: If standing in the Interactable of cover, signals that cover to enter it.
        protected void Interact()
        {
            if (touchedInteractable && touchedInteractable.CompareTag("Interactable")) 
            {
                Debug.Log("im on it");
                GameObject interactable = touchedInteractable.transform.parent.gameObject;
                interactable.GetComponent<InteractableClass>().Use(gameObject, touchedInteractable);
            }
        }

        private void OnTriggerEnter(Collider other)
        { 
            if (other.isTrigger && other.gameObject.CompareTag("Interactable")) touchedInteractable = other.gameObject;
        }
        private void OnTriggerExit(Collider other) 
        {
            if (cover != null) return;
            touchedInteractable = null; 
        }

    }
}
