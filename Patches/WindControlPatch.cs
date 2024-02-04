using System;
using System.Collections.Generic;
using System.Text;
using Game.Simulation;
using Game;
using HarmonyLib;
using Game.Rendering;
using Colossal.AssetPipeline;
using Colossal.AssetPipeline.PostProcessors.Wind;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;
using TreeWindController.Systems;
using Colossal.IO.AssetDatabase;

namespace TreeWindController.Patches {
    [HarmonyPatch(typeof(WindControl), "SetGlobalProperties")]
    public class WindControl_SetGlobalPropertiesPatch {

        public static bool Prefix(WindControl __instance, CommandBuffer __0, WindVolumeComponent __1) {
            // TODO Find a better way of grabbing the SettingsSystem. Doesn't
            // look like anything in WindControl gives us a way to get systems.
            var settings = SettingsSystem.Instance;
            if (settings == null) {
                return true;
            }

            settings.updateWindVolumeComponent(__1);
            // Pass through to the original method
            return true;
        }
    }
}
