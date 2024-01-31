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

        public ClampedFloatParameter strength;

        public static SettingsSystem Instance { get; private set; }

        protected override void OnCreate() {
            base.OnCreate();

            Instance = this;

            _log = Mod.Instance.Log;
            _log.Info("SettingsSystem.OnCreate");

            strength = new ClampedFloatParameter(0f, 0f, 5f);
        }

        protected override void OnUpdate() {
       }
    }
}
