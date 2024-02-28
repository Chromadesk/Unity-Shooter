using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class GunStandard : Gun
    {
        protected override void OnAwake()
        {
            setStats(Acooldown: 0.1f, AmaxAmmo: 6, AreloadTime: 0.45f, Adamage: 40f, AbulletVelocity: 40f);
        }
    }
}