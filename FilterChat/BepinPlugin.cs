using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using VoidManager;
using VoidManager.MPModChecks;
using Gameplay.Chat;

namespace FilterChat
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

    [HarmonyPatch(typeof(TextChat), "IncomingMessage")]
    public static class TextChatPatch
    {
        static void Prefix(string cloudID, string channelTextMessage)
        {
            BepinPlugin.Log.LogInfo($"Incoming message from {cloudID}: {channelTextMessage}");
        }
    }

    public class VoidManagerPlugin : VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Client;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;
    }
}
