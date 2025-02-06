using CG.Ship.Hull;
using Gameplay.CompositeWeapons;
using HarmonyLib;
using UnityEngine;

namespace EjectMagazine
{
    [HarmonyPatch(typeof(BulletMagazine), "ConsumeContainer")]
    internal class BulletMagazinePatch
    {
        static bool Prefix(CarryablesSocket socket)
        {
            socket.EjectCarryable(socket.transform.rotation * Vector3.back * 10);
            return false;
        }
    }
}
