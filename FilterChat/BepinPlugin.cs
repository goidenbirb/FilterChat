using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using VoidManager;
using VoidManager.MPModChecks;
using WebSocketSharp;

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

    [HarmonyPatch(typeof(Gameplay.Chat.TextChat), "IncomingMessage")]
    public static class ChatPatch
    {
        static bool Prefix(ref string cloudID, ref string channelTextMessage)
        {
            if (IsPlayer(cloudID))
            {
                return true;
            }
            return false;
        }

        private static bool IsPlayer(string cloudID)
        {
            var player = VoipService.CloudIDToPlayer(cloudID);
            return player != null && player.UserId != "System";
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