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
        //Copy this into OnAwake and replace ? with the ability type & class: user.ability? = gameObject.GetComponent<?>();
        //In time I will fix this. But now, I sleep.

        protected EntityClass user;
        protected float cooldown;
        protected bool onCooldown = false;
        protected CombatUI combatUI;

        protected void setStats(float Acooldown)
        {
            cooldown = Acooldown;
        }

        private void Awake()
        {
            user = gameObject.GetComponentInParent<EntityClass>();
            combatUI = gameObject.GetComponentInParent<CombatUI>();
            OnAwake();
        }

        public abstract void Use();
        protected abstract void OnAwake();

        public virtual void AltUse(KeyCode key) { }
        public virtual void OnDamageDealt(float damage) { }
        public virtual void OnDamageRecieved(float damage) { }

        protected void ResetCooldown() { onCooldown = false; }
    }

}