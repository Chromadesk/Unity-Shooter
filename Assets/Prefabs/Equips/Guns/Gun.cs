using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

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
        [NonSerialized] public int maxAmmo;
        [NonSerialized] public int currentAmmo;
        protected float reloadTime;
        [NonSerialized] public float reloadTimeFull;
        protected float damage;
        protected float bulletVelocity;
        [NonSerialized] public bool isReloading = false;

        //UI
        [SerializeField] GameObject UIMagObj;
        Image UIMag;
        List<Image> UIBullets = new();
        int UIBulletIndex = 0;
        bool doesUIMagSpin;

        protected void setStats(bool AdoesUIMagSpin, float Acooldown, int AmaxAmmo, float AreloadTime,
            float Adamage, float AbulletVelocity)
        {
            doesUIMagSpin = AdoesUIMagSpin;
            cooldown = Acooldown;
            maxAmmo = AmaxAmmo;
            currentAmmo = AmaxAmmo;
            reloadTime = AreloadTime;
            damage = Adamage;
            bulletVelocity = AbulletVelocity;

            reloadTimeFull = AreloadTime * AmaxAmmo;
            projScale = projectile.transform.localScale;

            SetupUIMag();
        }

        public override void Use()
        {
            if (onCooldown) return;
            if (currentAmmo <= 0) { emptySound.Play(); return; }

            GameObject bullet;

            //If the shooter is too close to a wall, don't spawn a bullet.
            if (!isReloading && !Physics.Raycast(user.transform.position, user.transform.forward, out RaycastHit hit, projScale.z))
            {
                bullet = fireProjectile();
                RemoveAmmo();
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

        public void StartReload()
        {
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
            SpinMag(reloadEnd.clip.length);
            reloadEnd.Play();
            yield return new WaitForSeconds(reloadEnd.clip.length);
            isReloading = false;
        }

        void AddAmmo()
        {
            currentAmmo += 1;
            reloadSound.Play();
            AddUIAmmo(reloadTime);
        }

        void RemoveAmmo()
        {
            currentAmmo -= 1;
            RemoveUIAmmo(cooldown);
        }

        void SetupUIMag()
        {
            UIMagObj = Instantiate(UIMagObj);
            ParentUIObjects(new List<GameObject>() { UIMagObj });

            UIMag = UIMagObj.GetComponent<Image>();
            for (int i = 0; i < UIMagObj.transform.childCount; i++)
            {
                UIBullets.Add(UIMagObj.transform.GetChild(i).GetComponent<Image>());
            }
        }

        void RemoveUIAmmo(float cooldownTime)
        {
            UIBullets[UIBulletIndex].enabled = false;
            UIMag.rectTransform.LeanRotateZ(Mathf.Floor(UIMag.rectTransform.eulerAngles.z + 60), cooldownTime - 0.05f);
            UIBulletIndex++;
        }

        void AddUIAmmo(float rotationTime)
        {
            UIBulletIndex--;
            UIBullets[UIBulletIndex].enabled = true;
            UIMag.rectTransform.LeanRotateZ(Mathf.Floor(UIMag.rectTransform.eulerAngles.z - 60), rotationTime - 0.05f);
        }

        int spins = 0;
        float spinTime;
        void SpinMag(float aSpinTime)
        {
            spinTime = aSpinTime / 4;
            SpinLoop();
        }

        void SpinLoop()
        {
            float cylinderZ = UIMag.rectTransform.eulerAngles.z;
            UIMag.rectTransform.LeanRotateZ(Mathf.Floor(cylinderZ + 180), spinTime - 0.02f); //Added to prevent overspinning
            spins++;

            if (spins < 2) { Invoke(nameof(SpinLoop), spinTime); return; }

            spins = 0;
        }
    }
}