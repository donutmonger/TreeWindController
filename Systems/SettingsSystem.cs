using System;
using System.Collections.Generic;
using System.Text;
using Colossal.Logging;
using Game;
using Game.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Unity.Mathematics;

namespace TreeWindController.Systems {
    internal class SettingsSystem : GameSystemBase {
        private ILog _log;

        public bool disableAllWind;

        public ClampedFloatParameter strength;
        public ClampedFloatParameter strengthVariance;
        public ClampedFloatParameter strengthVariancePeriod;
        private AnimationCurveParameter strengthVarianceAnimation;

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

            // TODO add separate clamped param for gust strength (it needs different bounds than base)
            strength = new ClampedFloatParameter(0.25f, 0f, 2f);
            strengthVariance = new ClampedFloatParameter(0f, 0f, 1f);
            strengthVariancePeriod  = new ClampedFloatParameter(6f, 0.01f, 120f);

            strengthVarianceAnimation = new AnimationCurveParameter( new AnimationCurve( ) );

            direction = new ClampedFloatParameter(65f, 0f, 360f);
            directionVariance = new ClampedFloatParameter(25f, 0f, 180f);
            directionVariancePeriod = new ClampedFloatParameter(15f, 0.01f, 120f);
        }

        protected override void OnUpdate() { }

        public void updateWindVolumeComponent(WindVolumeComponent w) {
            if (disableAllWind) {
                w.windGlobalStrengthScale.Override(0);
                w.windGlobalStrengthScale2.Override(0);
                return;
            }


            var minStrength = strength.value - strengthVariance.value * strength.value;
            var maxStrength = strength.value + strengthVariance.value * strength.value;

            strengthVarianceAnimation.value.SetKeys([
                    new Keyframe(0f, minStrength), 
                    new Keyframe(strengthVariancePeriod.value, maxStrength), 
                    new Keyframe(2*strengthVariancePeriod.value, minStrength)
            ]);

            var baseStrength = strengthVarianceAnimation.value.Evaluate(
                UnityEngine.Time.time % (2*strengthVariancePeriod.value)
            );
            var gustStrength = math.min(strength.max, 2 * baseStrength);


            w.windTreeBaseStrength.Override(baseStrength);
            w.windTreeGustStrength.Override(gustStrength);
            w.windTreeGustStrengthControl.value.SetKeys([new Keyframe(0f, 0f), new Keyframe(10f, gustStrength)]);

            w.windDirection.Override(direction.value);
            w.windDirectionVariance.Override(directionVariance.value);
            w.windDirectionVariancePeriod.Override(directionVariancePeriod.value);

            w.windParameterInterpolationDuration.Override(0.0001f);

            return;
        }

    }
}
