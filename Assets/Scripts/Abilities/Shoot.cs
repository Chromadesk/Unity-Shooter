using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class Shoot : Ability
    {
        [SerializeField] GameObject projectile;
        [SerializeField] GameObject gunSmokeParticle;
        [SerializeField] AudioSource fireSound;

        int maxAmmo = 6;
        int currentAmmo = 6;
        float reloadTime = 0.45f;
        bool isReloading = false;

        const float BULLET_VELOCITY = 40f;
        const float DAMAGE = 40f;
        readonly new float cooldown = 0.1f;
        Vector3 projScale;

        protected override void OnAwake()
        {
            user.abilityRanged = gameObject.GetComponent<Shoot>();
            projScale = projectile.transform.localScale;
        }

        public override void Use()
        {
            if (onCooldown) return;

            //If the shooter is too close to a wall, don't spawn a bullet.
            if (!Physics.Raycast(user.transform.position, user.transform.forward, out RaycastHit hit, projScale.z))
            {
                Vector3 pos = user.transform.position;
                GameObject bullet = Instantiate(projectile, user.transform.forward * 1f + user.transform.position, user.transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(user.transform.forward * BULLET_VELOCITY, ForceMode.VelocityChange);
                bullet.GetComponent<Projectile>().shooter = user.gameObject;
                bullet.GetComponent<Projectile>().damage = DAMAGE;
            }

            //Bullet effects
            GameObject smoke = Instantiate(gunSmokeParticle, user.transform);
            smoke.GetComponent<ParticleSystem>().Play();
            fireSound.Play();

            //Start cooldown
            onCooldown = true;
            Invoke(nameof(ResetCooldown), cooldown);
        }
    }
}