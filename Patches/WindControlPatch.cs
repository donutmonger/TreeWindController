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

            if (settings.disableAllWind) {
                __1.windGlobalStrengthScale.Override(0);
                __1.windGlobalStrengthScale2.Override(0);
                return true;
            }

            __1.windGlobalStrengthScale.Override(settings.strength.value);
            __1.windGlobalStrengthScale2.Override(settings.strength.value);

            // TODO this doesn't seem like it's doing anything
            __1.windTreeBaseStrengthVariancePeriod.Override(settings.strengthVariancePeriod.value);

            __1.windDirection.Override(settings.direction.value);
            __1.windDirectionVariance.Override(settings.directionVariance.value);
            __1.windDirectionVariancePeriod.Override(settings.directionVariancePeriod.value);
                

            // Pass through to the original method
            return true;
        }
    }
}
