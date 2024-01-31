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
            // TODO this is kind of gross, is there a nicer way to pass this ref through to the patch?
            var settings = SettingsSystem.Instance;
            if (settings == null) {
                return true;
            }

            // TODO instead of going this way, we should be able to get the full WindVolumeComponent from the settings system
            __1.windGlobalStrengthScale = settings.strength;
            __1.windGlobalStrengthScale2 = settings.strength;
            __1.windDirection = settings.direction;

            // Pass through to the original method
            return true;
        }
    }
}
