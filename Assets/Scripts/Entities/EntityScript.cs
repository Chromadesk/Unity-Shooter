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

        GameObject touchedInteractable;
        protected Rigidbody rB;

        [NonSerialized] public bool isStanding = true;
        [NonSerialized] public Cover cover = null;
        [NonSerialized] public int id;
        static int nextId = 0;
        bool damageDebounce = false;

        //Abilties
        public Gun gun;
        public Ability ability;
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
            gun.OnDamageDealt(damage);
            ability.OnDamageDealt(damage);
        }
        protected void OnDamageRecieved(float damage)
        {
            gun.OnDamageRecieved(damage);
            ability.OnDamageRecieved(damage);
        }
        void ResetDamageDebounce() { damageDebounce = false; }
        protected virtual void OnAwake() { }

    }
}
