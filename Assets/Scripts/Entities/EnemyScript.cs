using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

namespace Scripts
{
    public class EnemyScript : EntityClass
    {
        [SerializeField] Transform player;
        [SerializeField] LayerMask playerLayer;
        Vector3 direction;

        //States
        [SerializeField] float alertRange = 15f;
        [SerializeField] float attackRange = 10f;
        [SerializeField] float spaceBuffer = 3f;
        bool viewBlocked, moveStop;
        bool isAlerted, hasBeenAlerted, inAttackRange, inSpaceBuffer = false;
        float timePlayerSeen;

        private void FixedUpdate()
        {
            DecideAction();
        }

        protected override void OnDied()
        {
            Destroy(gameObject);
        }

        void DecideAction()
        {
            //Idle until the AI is alerted by the player.
            viewBlocked = Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, alertRange);
            if (!isAlerted) { 
                if (!hasBeenAlerted && hit.collider && hit.collider.gameObject.CompareTag("Player"))
                {
                    hasBeenAlerted = true;
                    rB.velocity = new Vector3(0, 1.5f, 0);
                    transform.LookAt(player);
                    Invoke(nameof(ResetIsAlerted), 0.5f);
                }
                return;
            }

            transform.LookAt(player);

            inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            inSpaceBuffer = Physics.CheckSphere(transform.position, spaceBuffer, playerLayer);
            if (!inSpaceBuffer && !moveStop) AiFollow(player);
            if (inAttackRange && !hasAttacked) 
            { 
                FireProjectile();
                moveStop = true;
                Invoke(nameof(ResetMoveStop), 0.3f);
            }
        }

        void ResetMoveStop() { moveStop = false; }

        void ResetIsAlerted() { isAlerted = true; }

        void AiFollow(Transform target)
        {
            direction = (target.position - transform.position).normalized;
            rB.MovePosition(transform.position + (moveSpeed * Time.deltaTime * direction));
        }

        void FindCover()
        {
            //TODO
        }
    }
} 