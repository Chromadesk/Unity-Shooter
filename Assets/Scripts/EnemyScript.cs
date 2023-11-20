using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    public class EnemyScript : EntityClass
    {
        [SerializeField] Transform player;
        [SerializeField] LayerMask playerLayer;
        Rigidbody rB;
        Vector3 direction;

        //Attacking
        [SerializeField] float attackCooldown = 3.5f;
        bool alreadyAttacked;

        //States
        [SerializeField] float attackRange = 3f;
        [SerializeField] float sightRange = 30f;
        bool canSeePlayer, canAttackPlayer;

        void Start()
        {
            rB = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            DecideAction();
        }

        protected override void OnDied()
        {
            Destroy(gameObject);
        }

        bool hasAttacked = false;
        void DecideAction()
        {
            transform.LookAt(player);
            canSeePlayer = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            canAttackPlayer = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            if (canSeePlayer && !canAttackPlayer) AiFollow(player);
            if (canAttackPlayer && !hasAttacked) 
            { 
                FireProjectile(); 
                hasAttacked = true;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }

        void ResetAttack() { hasAttacked = false; }

        void AiFollow(Transform target)
        {
            direction = target.position - transform.position;
            direction.Normalize();
            rB.MovePosition(transform.position + (moveSpeed * Time.deltaTime * direction));
        }

        void FindCover()
        {
            //TODO
        }
    }
} 