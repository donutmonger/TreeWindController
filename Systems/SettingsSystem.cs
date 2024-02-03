using System;
using System.Collections.Generic;
using System.Text;
using Colossal.Logging;
using Game;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace TreeWindController.Systems {
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
            strength = new ClampedFloatParameter(0f, 0f, 5f);
            strengthVariancePeriod  = new ClampedFloatParameter(6f, 0.01f, 120f);
            direction = new ClampedFloatParameter(65f, 0f, 360f);
            directionVariance = new ClampedFloatParameter(25f, 0f, 180f);
            directionVariancePeriod = new ClampedFloatParameter(15f, 0.01f, 120f);
        }

        protected override void OnUpdate() {
       }
    }
}
