using BepInEx;
using Colossal.PSI.Common;
using Game.Common;
using Game;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Game.Buildings;
using TreeWindController.Systems;
using Colossal.UI;
using HookUILib.Core;

#if BEPINEX_V6
    using BepInEx.Unity.Mono;
#endif

namespace TreeWindController {
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin {
        // IMod instance reference.
        private Mod _mod;

        private void Awake() {
            // Ersatz IMod.OnLoad().
            _mod = new();
            _mod.OnLoad();

            _mod.Log.Info("Plugin.Awake");

            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
            _mod.Log.Info("Harmony.CreateAndPatchAll done");
            var patchedMethods = harmony.GetPatchedMethods().ToArray();
            foreach (var patchedMethod in patchedMethods) {
                _mod.Log.Info($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
            }

        }

        /// <summary>
        /// Harmony postfix to <see cref="SystemOrder.Initialize"/> to substitute for IMod.OnCreateWorld.
        /// </summary>
        /// <param name="updateSystem"><see cref="GameManager"/> <see cref="UpdateSystem"/> instance.</param>
        [HarmonyPatch(typeof(SystemOrder), nameof(SystemOrder.Initialize))]
        [HarmonyPostfix]
        private static void InjectSystems(UpdateSystem updateSystem) => Mod.Instance.OnCreateWorld(updateSystem);
    }

    public class TreeWindControllerUI : UIExtension {
        public new readonly string extensionID = "tree-wind-controller";
        public new readonly string extensionContent;
        public new readonly ExtensionType extensionType = ExtensionType.Panel;

        public TreeWindControllerUI() {
            this.extensionContent = this.LoadEmbeddedResource("TreeWindController.dist.bundle.js");
        }
    }
}
