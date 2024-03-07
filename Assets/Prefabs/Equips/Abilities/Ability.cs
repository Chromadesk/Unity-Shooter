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
        [NonSerialized] public float cooldown;
        [NonSerialized] public bool onCooldown = false;

        //Sounds
        [SerializeField] List<GameObject> SFXObjects;
        protected Dictionary<string, GameObject> SFXInstances = new();

        protected void setStats(float Acooldown)
        {
            cooldown = Acooldown;
        }

        private void Awake()
        {
            user = gameObject.GetComponentInParent<EntityClass>();
            OnAwake();
        }

        protected void ParentUIObjects(List<GameObject> UIObjects)
        {
            if (!user.canvas) return;

            foreach (GameObject uiObj in UIObjects) 
            { 
                uiObj.transform.SetParent(user.canvas.transform, false);
            }
        }

        protected void ParentSFXObjects()
        {
            foreach (GameObject sfxObj in SFXObjects)
            {
                SFXInstances.Add(sfxObj.name, Instantiate(sfxObj));
                SFXInstances[sfxObj.name].transform.SetParent(transform, false);
            }
        }

        public abstract void Use();
        protected abstract void OnAwake();
        protected abstract void SetupSFX();
        public virtual void AltUse(KeyCode key) { }
        public virtual void OnDamageDealt(float damage) { }
        public virtual void OnDamageRecieved(float damage) { }

        protected void ResetCooldown() { onCooldown = false; }
    }

}