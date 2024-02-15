using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactable;
using UnityEditorInternal;
using System;
using Abilities;

namespace Scripts
{
    public abstract class EntityClass : MonoBehaviour
    {
        [SerializeField] protected string displayName;
        [SerializeField] protected float health = 100;
        [SerializeField] public readonly float maxHealth = 100;
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float attackDamage = 40f;
        [SerializeField] protected float attackCooldown;
        [SerializeField] GameObject projectile;
        [SerializeField] protected GameObject gunSmokeParticle;
        [SerializeField] protected float projectileVelocity;
        [SerializeField] AudioSource fireSound;

        GameObject touchedInteractable;

        protected Rigidbody rB;
        protected bool hasAttacked = false;
        protected Vector3 projScale;

        [NonSerialized] public bool isStanding = true;
        [NonSerialized] public Cover cover = null;
        [NonSerialized] public int id;
        static int nextId = 0;
        bool damageDebounce = false;

        //Abilties
        public Ability abilityMelee;
        public Ability abilityRanged;
        public Ability abilitySpecial;

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public float Health 
        {
            get => health;
            set 
            { 
                health = Mathf.Round(value); 
                if (health <= 0) OnDied(); 
                OnHealthSet(health);
            } 
        }

        private void Awake()
        {
            id = nextId;
            nextId++;

            projScale = projectile.transform.localScale;

            rB = GetComponent<Rigidbody>();

            OnAwake();
        }

        public void TakeDamage(float damage)
        {
            if (damageDebounce) return;
            Health -= damage;
            damageDebounce = true;
            OnDamageRecieved(damage);
            Invoke(nameof(ResetDamageDebounce), 0.01f);
        }

        protected void FireProjectile()
        {
            if (!isStanding || hasAttacked) return;
            
            //If the shooter is too close to a wall, don't spawn a bullet.
            if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, projScale.z))
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
            Invoke(nameof(ResetHasAttacked), attackCooldown);
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
        private void OnTriggerExit(Collider other) 
        {
            if (cover != null) return;
            touchedInteractable = null; 
        }

        protected abstract void OnDied();
        protected virtual void OnHealthSet(float value) { }
        public void OnDamageDealt(float damage)
        {
            if (abilityMelee) abilityMelee.OnDamageDealt(damage);
            if (abilityRanged) abilityRanged.OnDamageDealt(damage);
            if (abilitySpecial) abilitySpecial.OnDamageDealt(damage);
        }
        protected void OnDamageRecieved(float damage)
        {
            if (abilityMelee) abilityMelee.OnDamageRecieved(damage);
            if (abilityRanged) abilityRanged.OnDamageRecieved(damage);
            if (abilitySpecial) abilitySpecial.OnDamageRecieved(damage);
        }
        void ResetHasAttacked() { hasAttacked = false; }
        void ResetDamageDebounce() { damageDebounce = false; }
        protected virtual void OnAwake() { }

    }
}
