using Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class PlayerScript : EntityClass
    {
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

            if (Input.GetButtonDown("Fire1")) FireProjectile();
            if (Input.GetButtonDown("Interact")) Interact();
            if (Input.GetButtonDown("Fire2")) AltFire(true);
            if (Input.GetButtonUp("Fire2")) AltFire(false);
        }

        protected override void OnDied()
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            Invoke(nameof(ReloadLevel), 1.5f);
        }

        void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void AltFire(bool isDown)
        {
            if (cover != null) cover.ChangeStandCrouch(isDown);
        }
    }
}