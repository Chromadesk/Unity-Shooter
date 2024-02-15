using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        protected EntityClass user;
        protected float cooldown;
        protected bool onCooldown = false;
        protected readonly Type classType;
        protected readonly string abilityType;

        public Ability(Type classType, string abilityType)
        {
            this.classType = classType;
            this.abilityType = abilityType;
        }

        private void Awake()
        {
            user = gameObject.GetComponent<EntityClass>();
            OnAwake();
        }

        public abstract void Use();
        protected virtual void OnAwake() { }

        public virtual void OnDamageDealt(float damage) { }
        public virtual void OnDamageRecieved(float damage) { }

        protected void ResetCooldown() { onCooldown = false; }
    }

}