using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class PlayerScript : EntityClass
    {
        Rigidbody rB;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask ground;

        // Start is called before the first frame update
        void Start()
        {
            rB = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            RunControls();
        }

        void RunControls()
        {
            if (health <= 0) return;
            float inputHorz = Input.GetAxis("Horizontal");
            float inputVert = Input.GetAxis("Vertical");

            rB.velocity = new Vector3(inputHorz * moveSpeed, rB.velocity.y, inputVert * moveSpeed);

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rB.velocity = new Vector3(rB.velocity.x, jumpPower, rB.velocity.z);
            }
        }

        bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
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