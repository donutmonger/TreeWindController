using System;
using System.Collections.Generic;
using System.Text;
using Colossal.Logging;
using Game;
using Game.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace TreeWindController.Systems {
    // TODO this doesn't need to be a system, it's just a settings holder and helper for applying changes to the WindVolumeComponent
    internal class SettingsSystem : GameSystemBase {
        private ILog _log;

        public bool disableAllWind;

        public ClampedFloatParameter strength;
        public ClampedFloatParameter strengthVariancePeriod;

        public ClampedFloatParameter direction;
        public ClampedFloatParameter directionVariance;
        public ClampedFloatParameter directionVariancePeriod;

        public static SettingsSystem Instance { get; private set; }

        protected override void OnCreate() {
            base.OnCreate();

            Instance = this;

            _log = Mod.Instance.Log;
            _log.Info("SettingsSystem.OnCreate");

            disableAllWind = false;

            // Default values taken from WindVolumeComponent field defaults.
            strength = new ClampedFloatParameter(1f, 0f, 5f);
            strengthVariancePeriod  = new ClampedFloatParameter(6f, 0.01f, 120f);
            direction = new ClampedFloatParameter(65f, 0f, 360f);
            directionVariance = new ClampedFloatParameter(25f, 0f, 180f);
            directionVariancePeriod = new ClampedFloatParameter(15f, 0.01f, 120f);
        }

        protected override void OnUpdate() { }

        public WindVolumeComponent updateWindVolumeComponent(WindVolumeComponent w) {
            if (disableAllWind) {
                w.windGlobalStrengthScale.Override(0);
                w.windGlobalStrengthScale2.Override(0);
                return w;
            }

            w.windGlobalStrengthScale.Override(strength.value);
            w.windGlobalStrengthScale2.Override(strength.value);

            // TODO this doesn't seem like it's doing anything, maybe try other strength variance params?
            w.windTreeBaseStrengthVariancePeriod.Override(strengthVariancePeriod.value);

            w.windDirection.Override(direction.value);
            w.windDirectionVariance.Override(directionVariance.value);
            w.windDirectionVariancePeriod.Override(directionVariancePeriod.value);

            return w;
        }
    }
}
