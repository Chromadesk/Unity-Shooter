using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Cover : InteractableClass
    {
        protected override void Activate(GameObject activator)
        {
            Debug.Log("enter cover");
        }

        protected override void Deactivate()
        {
            Debug.Log("leave cover");
        }
    }
}
