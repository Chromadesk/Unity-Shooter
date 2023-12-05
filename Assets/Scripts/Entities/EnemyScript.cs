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
        float timePlayerSeen;

        //States
        [SerializeField] float alertRange = 10f;
        [SerializeField] float attackRange = 6f;
        [SerializeField] float spaceBuffer = 3f;
        bool moveStop, isAlerted, hasJumped, inAttackRange, inSpaceBuffer = false;
        RaycastHit[] nearbyAI = new RaycastHit[10];

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
            _ = Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, alertRange);

            //When the AI sees the player, they will jump and look at them for 0.5s, then stay alerted forever.
            if (!isAlerted) { 
                if (!hasJumped && hit.collider && hit.collider.gameObject.CompareTag("Player"))
                {
                    AlertAI();
                    AlertNearbyAI();
                }
                return;
            }

            transform.LookAt(player);

            TryAttackOrFollow();
        }

        public void AlertAI()
        {
            if (isAlerted) return;

            DoAlertJump();
            Invoke(nameof(ResetIsAlerted), 0.5f);
        }

        void DoAlertJump()
        {
            if (hasJumped) return; else hasJumped = true;
            rB.velocity = new Vector3(0, 1.5f, 0);
            transform.LookAt(player);
        }

        void AlertNearbyAI()
        {
            _ = Physics.SphereCastNonAlloc(transform.position, alertRange, transform.up, nearbyAI, 0.1f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (RaycastHit hit in nearbyAI) 
            {
                if (hit.collider == null) return;
                hit.collider.gameObject.SendMessage("AlertAI");
            }
        }

        void TryAttackOrFollow()
        {
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