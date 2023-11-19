using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class EnemyScript : EntityClass
    {
        public Transform player;
        Rigidbody rB;
        Vector3 movement;

        void Start()
        {
            rB = GetComponent<Rigidbody>();
        }

        void Update()
        {
            //transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);
            AiFollow(player);
        }

        private void FixedUpdate()
        {
            MoveEnemy(movement);
        }

        protected override void OnDied()
        {
            Destroy(gameObject);
        }

        void AiFollow(Transform target)
        {
            Vector3 direction = target.position - transform.position;
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //rB.rotation = angle;
            direction.Normalize();
            movement = direction;
        }

        void MoveEnemy(Vector3 direction)
        {
            rB.MovePosition(transform.position + (moveSpeed * Time.deltaTime * direction));
        }
    }
}