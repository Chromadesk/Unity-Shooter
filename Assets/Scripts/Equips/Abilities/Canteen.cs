using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Abilities
{
    public class Canteen : Ability
    {
        public readonly int maxCharge = 40;
        readonly int maxOverCharge = 80;
        public int currentCharge = 0;
        readonly float damageDealtCharge = 0.1f; //Percentage
        readonly float damageRecievedCharge = 0.1f; //Percentage
        readonly new float cooldown = 5;
        float decayTime;

        protected override void OnAwake()
        {
            user.ability = gameObject.GetComponent<Canteen>();
            decayTime = Time.time;
        }

        private void Update()
        {
            DecayOverharge();
        }

        public override void Use()
        {
            if (currentCharge <= 0 || onCooldown) return;

            if (user.Health == user.maxHealth) return;
            if (user.Health + currentCharge > user.maxHealth) user.Health = user.maxHealth;
            else user.Health += currentCharge;

            currentCharge = 0;
            onCooldown = true;
            Invoke(nameof(ResetCooldown), cooldown);
        }

        public void Charge(float amount, bool damDealt)
        {
            if (currentCharge >= maxOverCharge) return;

            float chargeMultiplier;

            if (damDealt) chargeMultiplier = damageDealtCharge; else chargeMultiplier = damageRecievedCharge;

            int addCharge = (int)Mathf.Round(amount * chargeMultiplier);

            if (addCharge + currentCharge > maxOverCharge) currentCharge = maxOverCharge; else currentCharge += addCharge;
        }

        void DecayOverharge()
        {
            if (currentCharge <= maxCharge || Time.time - decayTime < 1) return;
            currentCharge -= 1;
            decayTime = Time.time;
        }

        public override void OnDamageDealt(float damage) { Charge(damage, true); }
        public override void OnDamageRecieved(float damage) { Charge(damage, false); }
    }
}