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
        [SerializeField] AudioSource reloadSound;
        [SerializeField] AudioSource emptySound;
        [SerializeField] AudioSource reloadEnd;

        int maxAmmo = 6;
        int currentAmmo = 6;
        float reloadTime = 0.45f;
        bool isReloading = false;

        [SerializeField] CombatUI combatUI;

        void Update()
        {
            if (health <= 0) return;
            FaceMouse();
            RunControls();
        }

        void FaceMouse()
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Camera.main.transform.position.y));
            transform.LookAt(new Vector3(mouseWorld.x, transform.position.y, mouseWorld.z));
        }

        void RunControls()
        {
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");
            rB.velocity = new Vector3(inputHorz * moveSpeed, rB.velocity.y, inputVert * moveSpeed);

            //Camera follows the player
            Camera.main.transform.position = new Vector3(
                transform.position.x,
                Camera.main.transform.position.y, 
                transform.position.z);

            if (Input.GetButtonDown("Interact")) Interact();
            if (Input.GetButtonDown("Fire2")) AltFire(true);
            if (Input.GetButtonUp("Fire2")) AltFire(false);

            if (Input.GetButtonDown("Reload"))
            {
                if (currentAmmo == maxAmmo) return;
                if (isReloading) 
                { 
                    StopCoroutine(Reload()); 
                    reloadEnd.Play(); 
                    isReloading = false; 
                    Debug.Log("reload stopped"); 
                    return;
                }
                StartCoroutine(Reload());
                Debug.Log("start reload");
            }
                
            if (Input.GetButtonDown("Fire1"))
            {
                if (isReloading || !isStanding) return;
                if (currentAmmo <= 0) { emptySound.Play(); return; }
                FireProjectile();
                RemoveAmmo();
            }
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

        void AltFire(bool isDown)
        {
            if (cover != null) cover.ChangeStandCrouch(isDown);
        }

        IEnumerator Reload()
        {
            isReloading = true;

            while (currentAmmo != maxAmmo && isReloading)
            {
                yield return new WaitForSeconds(reloadTime);
                if (!isReloading) yield break;
                AddAmmo();
                Debug.Log("currentammo = " + currentAmmo);
            }
            
            yield return new WaitForSeconds(reloadSound.clip.length);
            reloadEnd.Play();
            yield return new WaitForSeconds(reloadEnd.clip.length);
            isReloading = false;
            Debug.Log("reload over");
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
            combatUI.RemoveUIAmmo(reloadTime);
        }
    }
}