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

        private InputAction _increaseStrength;
        private InputAction _deceaseStrength;

        public ClampedFloatParameter strength;

        public static SettingsSystem Instance { get; private set; }

        protected override void OnCreate() {
            base.OnCreate();

            Instance = this;

            _log = Mod.Instance.Log;
            _log.Info("WindControlSettingsSystem.OnCreate");

            strength = new ClampedFloatParameter(0f, 0f, 5f);

            _increaseStrength = new("WindControlSettingsSystem-IncreaseStrength");
            _increaseStrength.AddCompositeBinding("ButtonWithOneModifier").With("Modifier", "<Keyboard>/ctrl").With("Button", "<Mouse>/rightButton");
            _increaseStrength.Enable();

            _deceaseStrength = new("WindControlSettingsSystem-DecreaseStrength");
            _deceaseStrength.AddCompositeBinding("ButtonWithOneModifier").With("Modifier", "<Keyboard>/ctrl").With("Button", "<Mouse>/leftButton");
            _deceaseStrength.Enable();
        }

        protected override void OnUpdate() {
            if (_increaseStrength.WasPressedThisFrame()) {
                strength.value += 0.1f;
                _log.Info($"{strength}");
            }

            if (_deceaseStrength.WasPressedThisFrame()) {
                strength.value -= 0.1f;
                _log.Info($"{strength}");
            }
        }
    }
}
