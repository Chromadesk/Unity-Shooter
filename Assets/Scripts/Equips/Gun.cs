using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public abstract class Gun : Ability
    {
        //Projectile
        [SerializeField] GameObject projectile;
        [SerializeField] GameObject gunSmokeParticle;
        Vector3 projScale;

        //Sounds
        [SerializeField] protected AudioSource reloadSound;
        [SerializeField] protected AudioSource reloadEnd;
        [SerializeField] protected AudioSource emptySound;
        [SerializeField] protected AudioSource fireSound;

        //Stats
        protected int maxAmmo;
        protected int currentAmmo;
        protected float reloadTime;
        protected float damage;
        protected float bulletVelocity;
        protected bool isReloading = false;

        protected void setStats(float Acooldown, int AmaxAmmo, float AreloadTime,
            float Adamage, float AbulletVelocity)
        {
            cooldown = Acooldown;
            maxAmmo = AmaxAmmo;
            currentAmmo = AmaxAmmo;
            reloadTime = AreloadTime;
            damage = Adamage;
            bulletVelocity = AbulletVelocity;
        }

        private void Awake()
        {
            user.gun = gameObject.GetComponent<GunStandard>();

            projScale = projectile.transform.localScale;
        }

        public override void Use()
        {
            if (onCooldown) return;
            if (currentAmmo <= 0) { emptySound.Play(); return; }

            GameObject bullet;

            //If the shooter is too close to a wall, don't spawn a bullet.
            if (!Physics.Raycast(user.transform.position, user.transform.forward, out RaycastHit hit, projScale.z))
            {
                bullet = fireProjectile();
            }
            else return;

            //Bullet effects
            GameObject smoke = Instantiate(gunSmokeParticle, user.transform);
            smoke.GetComponent<ParticleSystem>().Play();
            fireSound.Play();

            //Start cooldown
            onFire(bullet);
            onCooldown = true;
            Invoke(nameof(ResetCooldown), cooldown);
        }

        public override void AltUse(KeyCode key)
        {
            if (key.CompareTo(KeyCode.R) == 0) return;

            if (currentAmmo == maxAmmo) return;
            if (isReloading)
            {
                StopCoroutine(Reload());
                reloadEnd.Play();
                isReloading = false;
                return;
            }
            StartCoroutine(Reload());
        }

        protected virtual void onFire(GameObject bullet) { }

        GameObject fireProjectile()
        {
            Vector3 pos = user.transform.position;
            GameObject bullet = Instantiate(projectile, user.transform.forward * 1f + user.transform.position, user.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(user.transform.forward * bulletVelocity, ForceMode.VelocityChange);
            bullet.GetComponent<Projectile>().shooter = user.gameObject;
            bullet.GetComponent<Projectile>().damage = damage;

            return bullet;
        }

        protected IEnumerator Reload()
        {
            isReloading = true;

            //Reloads each individual bullet
            while (currentAmmo != maxAmmo && isReloading)
            {
                yield return new WaitForSeconds(reloadTime);
                if (!isReloading) yield break;
                AddAmmo();
            }

            //Delay & animation before reload truly ends
            yield return new WaitForSeconds(reloadSound.clip.length);
            combatUI.SpinCylinder(reloadEnd.clip.length);
            reloadEnd.Play();
            yield return new WaitForSeconds(reloadEnd.clip.length);
            isReloading = false;
        }

        void AddAmmo()
        {
            currentAmmo += 1;
            reloadSound.Play();
            combatUI.AddUIAmmo(reloadTime);
        }

        void RemoveAmmo()
        {
            currentAmmo -= 1;
            combatUI.RemoveUIAmmo(cooldown);
        }
    }
}