using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public abstract class InteractableClass : MonoBehaviour
    {
        public GameObject user;
        bool InUse = false;

        public void Use(GameObject activator, GameObject interactableUsed)
        {
            if (InUse)
            {
                Deactivate();
                user = null;
                InUse = false;
            }
            else 
            {
                user = activator;
                InUse = true;
                Activate(interactableUsed);
            }
        }

        protected abstract void Activate(GameObject interactableUsed);
        protected abstract void Deactivate();
    }
}
