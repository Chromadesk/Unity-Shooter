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

        public void Use(GameObject activator)
        {
            if (InUse)
            { 
                Deactivate(); 
                user = null; 
                InUse = false; 
            }
            else 
            { 
                Activate(activator);
                user = activator;
                InUse = true; 
            }
        }

        protected abstract void Activate(GameObject activator);
        protected abstract void Deactivate();
    }
}
