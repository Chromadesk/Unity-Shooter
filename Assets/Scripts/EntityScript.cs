using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public abstract class EntityClass : MonoBehaviour
    {
        [SerializeField] protected string displayName;
        [SerializeField] protected float health = 100;
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float jumpPower = 5f;

        public float Health { get => health; set { health = Mathf.Round(value); if (health <= 0) OnDied(); } }

        protected abstract void OnDied();

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }
    }
}