
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Scripts
{
    public class EnemyScript : EntityClass
    {
        [SerializeField] Transform player;
        [SerializeField] NavMeshAgent agent;

        //States
        [SerializeField] float alertRange = 10f;
        [SerializeField] float attackRange = 6f;
        [SerializeField] float spaceBuffer = 3f;
        bool isAlerted, hasJumped, canSeePlayer = false;
        RaycastHit[] nearbyAI = new RaycastHit[10];
        RaycastHit rayToPlayer;

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
            _ = Physics.BoxCast(
                transform.position,
                new Vector3(projScale.x / 2, projScale.y / 2, projScale.z / 2), 
                (player.position - transform.position).normalized,
                out rayToPlayer,
                transform.rotation,
                alertRange);

            if (rayToPlayer.collider && rayToPlayer.collider.gameObject.CompareTag("Player")) canSeePlayer = true; else canSeePlayer = false;

            //When the AI sees the player, they will jump and look at them for 0.5s, then stay alerted forever.
            if (!isAlerted) { 
                if (!hasJumped && canSeePlayer)
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

            rB.isKinematic = true;
            Invoke(nameof(ResetIsAlerted), 0.5f);
        }

        void AlertNearbyAI()
        {
            _ = Physics.SphereCastNonAlloc(transform.position, alertRange, transform.up, nearbyAI, 0.1f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (RaycastHit hit in nearbyAI) 
            {
                if (hit.collider == null) return;

                hit.collider.gameObject.SendMessage("AlertAI", SendMessageOptions.DontRequireReceiver);
                
            }
        }

        void TryAttackOrFollow()
        {
            if ((!canSeePlayer || rayToPlayer.distance > spaceBuffer) && !agent.isStopped) { agent.SetDestination(player.position); }

            if (rayToPlayer.distance <= attackRange && !hasAttacked && canSeePlayer)
            {
                print("tried attack");
                FireProjectile();
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                Invoke(nameof(ResetMoveStop), 0.3f);
            }
        }

        void ResetMoveStop() { agent.isStopped = false; }

        void ResetIsAlerted() { isAlerted = true; }

        void FindCover()
        {
            //TODO
        }

        protected override void OnHealthSet(float health)
        {
            if (!isAlerted) { AlertAI(); AlertNearbyAI(); }
        }
    }
} 