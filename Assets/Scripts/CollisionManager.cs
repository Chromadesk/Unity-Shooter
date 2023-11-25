using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
   public class CollisionManager : MonoBehaviour
    {
        public readonly static LayerMask playerLayer = LayerMask.NameToLayer("Player");
        public readonly static LayerMask enemyLayer = LayerMask.NameToLayer("Enemy");
        public readonly static LayerMask interactableLayer = LayerMask.NameToLayer("Interactable");
        public readonly static GameObject player = GameObject.Find("Player");


    }
}