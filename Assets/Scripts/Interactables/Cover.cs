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
            Debug.Log("enter cover");
            userEntity = user.GetComponent<EntityClass>();
            originalMoveSpeed = userEntity.MoveSpeed;
            userEntity.MoveSpeed = 0f;

            user.transform.position = new Vector3(
                interactableUsed.transform.position.x, 
                user.transform.position.y, 
                interactableUsed.transform.position.z);
        }

        protected override void Deactivate()
        {
            Debug.Log("leave cover");
            userEntity.MoveSpeed = originalMoveSpeed;
        }
    }
}
