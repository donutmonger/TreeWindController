using System;
using System.Collections.Generic;
using System.Text;
using Game.UI;
using Colossal.Annotations;
using Colossal.Logging;
using Colossal.UI.Binding;
using UnityEngine.Rendering;
using UnityEngine;
using TreeWindController.UI;
using UnityEngine.UIElements.Collections;

namespace TreeWindController.Systems {
    class SettingsUISystem : UISystemBase {
        public static string PanelID = "tree-wind-controller";

        private ILog _log;
        private Dictionary<string,IJsonWritable> _fields;
        private SettingsSystem _settings;

        protected override void OnCreate() {
            base.OnCreate();

            _log = Mod.Instance.Log;
            _log.Info("SettingsUISystem.OnCreate");

            _settings = World.GetExistingSystemManaged<SettingsSystem>();

            _fields = new Dictionary<string, IJsonWritable> {
                {
                    "disable_wind",
                    new Checkbox {
                        label = "Disable All Wind",
                        getChecked = new Func<bool>(() => { return _settings.disableAllWind; }),
                        setChecked = new Action<bool>((bool b) => { _settings.disableAllWind = b; }),
                    }
                },
                {
                    "wind_strength",
                    new Slider {
                        label = "Wind Strength",
                        unit = "%",
                        getValue = new Func<ClampedFloatParameter>(() => { return new ClampedFloatParameter(percentageClamped(_settings.strength), 0f, 100f); }),
                        setValue = new Action<float>((float f) => { _settings.strength.value = clampedValuePercent(_settings.strength, f); })
                    }
                },
                {
                    "wind_strength_variance",
                    new Slider {
                        label = "Wind Strength Variance",
                        unit = "%",
                        getValue = new Func<ClampedFloatParameter>(() => { return new ClampedFloatParameter(percentageClamped(_settings.strengthVariance), 0f, 100f); }),
                        setValue = new Action<float>((float f) => { _settings.strengthVariance.value = clampedValuePercent(_settings.strengthVariance, f); })
                    }
                },
                {
                    "wind_strength_variance_period",
                    new Slider {
                        label = "Wind Strength Variance Period",
                        unit = "s",
                        getValue = new Func<ClampedFloatParameter>(() => { return _settings.strengthVariancePeriod; }),
                        setValue = new Action<float>((float f) => { _settings.strengthVariancePeriod.value = f; })
                    }
                },
                {
                    "wind_direction",
                    new Slider {
                        label = "Wind Direction",
                        unit = "°",
                        getValue = new Func<ClampedFloatParameter>(() => { return _settings.direction; }),
                        setValue = new Action<float>((float f) => { _settings.direction.value = f; })
                    }
                },
                {
                    "wind_direction_variance",
                    new Slider {
                        label = "Wind Direction Variance",
                        unit = "%",
                        getValue = new Func<ClampedFloatParameter>(() => { return new ClampedFloatParameter(percentageClamped(_settings.directionVariance), 0f, 100f); }),
                        setValue = new Action<float>((float f) => { _settings.directionVariance.value = clampedValuePercent(_settings.directionVariance, f); })
                    }
                },
                {
                    "wind_direction_variance_period",
                    new Slider {
                        label = "Wind Direction Variance Period",
                        unit = "s",
                        getValue = new Func<ClampedFloatParameter>(() => { return _settings.directionVariancePeriod; }),
                        setValue = new Action<float>((float f) => { _settings.directionVariancePeriod.value = f; })
                    }
                }
            };

            this.AddUpdateBinding(
                new GetterValueBinding<Dictionary<string, IJsonWritable>>(
                    PanelID,
                    "get_values",
                    GetValues,
                    new NestedDictionaryWriter<string, IJsonWritable>(),
                    // TODO equality detection can be based on the result of getChecked/getValue.
                    new NeverEqualEqualityComparer<Dictionary<string,IJsonWritable>>()
                )
            ); 

            this.AddBinding(
                new TriggerBinding<string,bool>(PanelID, "set_bool_value", new Action<string,bool>(SetBoolValue))
            );
            this.AddBinding(
                new TriggerBinding<string,float>(PanelID, "set_value", new Action<string,float>(SetFloatValue))
            );
        }

        private Dictionary<string,IJsonWritable> GetValues() {
            return _fields;
        }

        private void SetBoolValue(string key, bool value) {
            // TODO This, and the setter for float values, is a bit gross.
            // There's probably a better covering interface to use over just
            // IJsonWritable for UI elements.
            try {
                var field = (Checkbox)_fields.Get(key);
                field.setChecked(value);
            } catch (InvalidCastException) { }
        }

        private void SetFloatValue(string key, float value) {
            try {
                var field = (Slider)_fields.Get(key);
                field.setValue(value);
            } catch (InvalidCastException) { }
        }

        private float percentageClamped(ClampedFloatParameter cfp) {
            return 100f * (cfp.value - cfp.min) / (cfp.max - cfp.min);
        }

        private float clampedValuePercent(ClampedFloatParameter cfp, float percent) {
            return cfp.min + percent / 100f * (cfp.max - cfp.min);
        }
    }

    internal class NeverEqualEqualityComparer<T> : EqualityComparer<T> {
        public override bool Equals(T x, T y) {
            return false;
        }

        public override int GetHashCode(T obj) {
            return obj.GetHashCode();
        }
    }
}
