using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class PlayerScript : EntityClass
    {
        Rigidbody rB;

        // Start is called before the first frame update
        void Start()
        {
            rB = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            FaceMouse();
            RunControls();
        }

        void FaceMouse()
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Camera.main.transform.position.y));
            transform.LookAt(mouseWorld);
        }

        void RunControls()
        {
            if (health <= 0) return;

            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");
            rB.velocity = new Vector3(inputHorz * moveSpeed, rB.velocity.y, inputVert * moveSpeed);

            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("fire");
                FireProjectile();
            }
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
    }
}