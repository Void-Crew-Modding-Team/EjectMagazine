using CG.Ship.Hull;
using Gameplay.CompositeWeapons;
using HarmonyLib;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace EjectMagazine
{
    [HarmonyPatch(typeof(BulletMagazine), "ConsumeContainer")]
    internal class BulletMagazinePatch
    {
        internal static List<Player> playersWithMod = new();

        static bool Prefix(BulletMagazine __instance, CarryablesSocket socket)
        {
            if (!VoidManagerPlugin.Enabled) return true;
            Player owner = __instance.WeaponBase.photonView.Owner;
            if (owner.IsLocal || playersWithMod.Contains(owner))
            {
                socket.EjectCarryable(socket.transform.rotation * Vector3.back * 10);
                return false;
            }
            return true;
        }
    }
}
