
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
        readonly float alertRange = 10f;
        readonly float attackRange = 10f;
        readonly float spaceBuffer = 3f;
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
            CheckIfPlayerInView();

            if (canSeePlayer) transform.LookAt(player);

            TryAttackOrFollow();
        }

        void TryAttackOrFollow()
        {
            if (!agent.isStopped && GetPlrDistance() > spaceBuffer) {
                agent.SetDestination(player.position); 
            }

            if (!gun.isReloading && gun.currentAmmo <= 0) Reload();

            if (rayToPlayer.distance <= attackRange 
                && !gun.onCooldown
                && canSeePlayer
                && !gun.isReloading
                && canAttack)
            {
                Attack();
            }
        }

        void Attack()
        {
            gun.Use();
            canAttack = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            Invoke(nameof(ResetMoveStop), 0.3f);
            Invoke(nameof(ResetCanAttack), gun.cooldown * Random.Range(2, 3) + 0.2f);
        }

        void Reload()
        {
            agent.speed = moveSpeed / 2;
            gun.StartReload();
            Invoke(nameof(DoAfterReload), gun.reloadTimeFull);
        }

        void DoAfterReload()
        {
            agent.speed = moveSpeed;
        }

        void CheckIfPlayerInView()
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
                return;
            }
            else canSeePlayer = false;
        }

        float GetPlrDistance()
        {
            return (player.transform.position - transform.position).magnitude;
        }

        void ResetMoveStop() { agent.isStopped = false; }
        void ResetCanAttack() { canAttack = true; }

        void FindCover()
        {
            //TODO
        }
    }
}