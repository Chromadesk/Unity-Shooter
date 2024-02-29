
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace Scripts
{
    public class EnemyScript : EntityClass
    {
        Transform player;
        [SerializeField] NavMeshAgent agent;

        //States
        float alertRange = 10f;
        float attackRange = 10f;
        float spaceBuffer = 3f;
        bool canSeePlayer = false;
        bool canAttack = true;
        RaycastHit rayToPlayer;

        private void FixedUpdate()
        {
            DecideAction();
        }

        protected override void OnDied()
        {
            Destroy(gameObject);
        }

        protected override void OnAwake()
        {
            player = GameObject.Find("Player").transform;
        }

        void DecideAction()
        {
            _ = Physics.BoxCast(
                transform.position,
                new Vector3(0.25f, 0.25f, 0.25f),
                (player.position - transform.position).normalized,
                out rayToPlayer,
                transform.rotation,
                alertRange);

            if (rayToPlayer.collider && rayToPlayer.collider.gameObject.CompareTag("Player"))
            {
                canSeePlayer = true;
            } 
            else canSeePlayer = false;

            if (canSeePlayer) transform.LookAt(player);

            TryAttackOrFollow();
        }

        void TryAttackOrFollow()
        {
            if ((!canSeePlayer || rayToPlayer.distance > spaceBuffer) && !agent.isStopped) { agent.SetDestination(player.position); }

            if (rayToPlayer.distance <= attackRange 
                && !gun.onCooldown
                && canSeePlayer
                && !gun.isReloading
                && canAttack)
            {
                Attack();
            }

            if (!gun.isReloading && gun.currentAmmo <= 0) Reload();
        }

        void Attack()
        {
            gun.Use();
            canAttack = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            Invoke(nameof(ResetMoveStop), 0.3f);
            Invoke(nameof(ResetCanAttack), gun.cooldown * Random.Range(4, 5) + 0.2f);
        }

        void Reload()
        {
            gun.StartReload();
        }

        void ResetMoveStop() { agent.isStopped = false; }
        void ResetCanAttack() { canAttack = true; }

        void FindCover()
        {
            //TODO
        }
    }
}