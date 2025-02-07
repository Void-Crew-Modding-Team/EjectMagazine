﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using System.Linq;
using System.Reflection;
using VoidManager;
using VoidManager.MPModChecks;

namespace EjectMagazine
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.USERS_PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency(VoidManager.MyPluginInfo.PLUGIN_GUID)]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }


    public class VoidManagerPlugin : VoidPlugin
    {
        public static bool Enabled { get; private set; }

        public VoidManagerPlugin()
        {
            Events.Instance.ClientModlistRecieved += (o, e) =>
            {
                BulletMagazinePatch.playersWithMod.RemoveAll(player => !PhotonNetwork.PlayerList.Contains(player));
                if (NetworkedPeerManager.Instance.NetworkedPeerHasMod(e.player, MyPluginInfo.PLUGIN_GUID))
                    BulletMagazinePatch.playersWithMod.Add(e.player);
            };
        }

        public override MultiplayerType MPType => MultiplayerType.Client;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;

        public override SessionChangedReturn OnSessionChange(SessionChangedInput input)
        {
            Enabled = input.IsMod_Session;
            return new SessionChangedReturn() { SetMod_Session = true };
        }
    }
}