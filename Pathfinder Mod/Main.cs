using UnityEngine;
using Harmony;
using UnityModManagerNet;

namespace TestMod
{
    static class Main
    {
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;

            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            modEntry.Logger.Log(Application.loadedLevelName);

            return true;
        }
    }

    [HarmonyPatch(typeof(Application), "loadedLevelName", MethodType.Getter)]
    static class Application_loadedLevelName_Patch
    {
        static void Postfix(ref string __result)
        {
            if (!Main.enabled)
                return;

            __result = "New Level Name";
        }
    }
}