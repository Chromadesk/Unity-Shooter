using Abilities;
using Interactable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class PlayerScript : EntityClass
    {
        //Sounds
        [SerializeField] AudioSource reloadSound;
        [SerializeField] AudioSource emptySound;
        [SerializeField] AudioSource reloadEnd;

        //UI
        [SerializeField] CombatUI combatUI;

        //Controls
        bool usingMelee = true;

        void Update()
        {
            if (Health <= 0) return;
            FaceMouse();
            MoveCameraToPlayer();
            RunControls();
            combatUI.DisplayAbility(abilityMelee);
            combatUI.DisplayAbility(abilityRanged);
            combatUI.DisplayAbility(abilitySpecial);
        }

        void FaceMouse()
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Camera.main.transform.position.y));
            transform.LookAt(new Vector3(mouseWorld.x, transform.position.y, mouseWorld.z));
        }

        void RunControls()
        {
            //Move controls
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");
            rB.velocity = new Vector3(inputHorz * moveSpeed, rB.velocity.y, inputVert * moveSpeed);

            if (Input.GetKeyDown(KeyCode.F)) abilitySpecial.Use();
            if (Input.GetButtonDown("Interact")) Interact();
            if (Input.GetKeyDown(KeyCode.Q)) { if (usingMelee) usingMelee = false; else usingMelee = true; }
            if (Input.GetButtonDown("Fire1"))
            {
                if (abilityRanged && !usingMelee) { abilityRanged.Use(); return; }
                if (abilityMelee) abilityMelee.Use();
            }

            //if (Input.GetButtonDown("Reload"))
            //{
            //    if (currentAmmo == maxAmmo) return;
            //    if (isReloading)
            //    {
            //        StopCoroutine(Reload());
            //        reloadEnd.Play();
            //        isReloading = false;
            //        return;
            //    }
            //    StartCoroutine(Reload());
            //}
        }

        void MoveCameraToPlayer()
        {
            Camera.main.transform.position = new Vector3(
                transform.position.x,
                Camera.main.transform.position.y,
                transform.position.z);
        }

        protected override void OnDied()
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            Invoke(nameof(ReloadLevel), 1.5f);
        }

        protected override void OnHealthSet(float value)
        {
            combatUI.DisplayHealth(value);
        }

        void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //IEnumerator Reload()
        //{
        //    isReloading = true;

        //    while (currentAmmo != maxAmmo && isReloading)
        //    {
        //        yield return new WaitForSeconds(reloadTime);
        //        if (!isReloading) yield break;
        //        AddAmmo();
        //    }
            
        //    yield return new WaitForSeconds(reloadSound.clip.length);
        //    combatUI.SpinCylinder(reloadEnd.clip.length);
        //    reloadEnd.Play();
        //    yield return new WaitForSeconds(reloadEnd.clip.length);
        //    isReloading = false;
        //}

        //void AddAmmo()
        //{
        //    currentAmmo += 1;
        //    reloadSound.Play();
        //    combatUI.AddUIAmmo(reloadTime);
        //}

        //void RemoveAmmo()
        //{
        //    currentAmmo -= 1;
        //    combatUI.RemoveUIAmmo(attackCooldown);
        //}
    }
}