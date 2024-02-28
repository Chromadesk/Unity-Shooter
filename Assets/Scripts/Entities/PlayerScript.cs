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
        //UI
        [SerializeField] CombatUI combatUI;

        void Update()
        {
            if (Health <= 0) return;
            FaceMouse();
            MoveCameraToPlayer();
            RunControls();
            combatUI.DisplayAbility(ability);
            //combatUI.DisplayAbility(gun);
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

            if (Input.GetKeyDown(KeyCode.E)) ability.Use();
            if (Input.GetButtonDown("Interact")) Interact();
            if (Input.GetButtonDown("Fire1")) gun.Use();
            if (Input.GetKeyDown(KeyCode.R)) gun.StartReload();
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
    }
}