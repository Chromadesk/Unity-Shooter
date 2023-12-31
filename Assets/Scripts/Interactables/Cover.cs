using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Cover : InteractableClass
    {
        float originalMoveSpeed;
        EntityClass userEntity;

        protected override void Activate(GameObject interactableUsed)
        {
            userEntity = user.GetComponent<EntityClass>();
            originalMoveSpeed = userEntity.MoveSpeed;
            userEntity.MoveSpeed = 0f;

            user.transform.position = new Vector3(
                interactableUsed.transform.position.x,
                user.transform.position.y / 1.5f,
                interactableUsed.transform.position.z);

            gameObject.layer = LayerMask.NameToLayer("Wall");
            userEntity.cover = this;
            userEntity.isStanding = false;
        }

        protected override void Deactivate()
        {
            userEntity.MoveSpeed = originalMoveSpeed;
            userEntity.cover = null;
            Stand();
        }

        void Crouch()
        {
            user.transform.position = new Vector3(
                user.transform.position.x,
                user.transform.position.y / 1.5f,
                user.transform.position.z);

            gameObject.layer = LayerMask.NameToLayer("Wall");
            userEntity.isStanding = false;
        }

        void Stand()
        {
            user.transform.position = new Vector3(
                user.transform.position.x,
                user.transform.position.y * 1.5f,
                user.transform.position.z);

            gameObject.layer = LayerMask.NameToLayer("BulletIgnore");
            userEntity.isStanding = true;
        }

        public void ChangeStandCrouch(bool standUp)
        {
            if (standUp) Stand(); else Crouch();

        }
    }
}
