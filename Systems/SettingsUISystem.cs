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
        private ILog _log;
        private string kGroup = "tree-wind-controller";

        private Dictionary<string,IJsonWritable> fields;
        private SettingsSystem _settings;

        protected override void OnCreate() {
            base.OnCreate();

            _log = Mod.Instance.Log;
            _log.Info("SettingsUISystem.OnCreate");

            _settings = World.GetExistingSystemManaged<SettingsSystem>();

            this.AddUpdateBinding(
                new GetterValueBinding<Dictionary<string,IJsonWritable>>(this.kGroup, "get_values", GetValues,
                new NestedDictionaryWriter<string, IJsonWritable>())
            );

            this.AddBinding(
                new TriggerBinding<string,bool>(kGroup, "set_bool_value", new Action<string,bool>(SetBoolValue))
            );
            this.AddBinding(
                new TriggerBinding<string,float>(kGroup, "set_value", new Action<string,float>(SetFloatValue))
            );
        }

        // TODO why doesn't this work when just making the dictionary once on OnCreate?
        private Dictionary<string,IJsonWritable> GetValues() {
            fields = new Dictionary<string, IJsonWritable> {
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
                        getValue = new Func<ClampedFloatParameter>(() => { return new ClampedFloatParameter(percentageClamped(_settings.strength), 0f, 100f); }),
                        unit = "%",
                        setValue = new Action<float>((float f) => { _settings.strength.value = setClampedValuePercent(_settings.strength, f); })
                    }
                },
                {
                    "wind_strength_variance",
                    new Slider {
                        label = "Wind Strength Variance",
                        getValue = new Func<ClampedFloatParameter>(() => { return new ClampedFloatParameter(percentageClamped(_settings.strengthVariance), 0f, 100f); }),
                        unit = "%",
                        setValue = new Action<float>((float f) => { _settings.strengthVariance.value = setClampedValuePercent(_settings.strengthVariance, f); })
                    }
                },
                {
                    "wind_strength_variance_period",
                    new Slider {
                        label = "Wind Strength Variance Period",
                        getValue = new Func<ClampedFloatParameter>(() => { return _settings.strengthVariancePeriod; }),
                        unit = "s",
                        setValue = new Action<float>((float f) => { _settings.strengthVariancePeriod.value = f; })
                    }
                },
                {
                    "wind_direction",
                    new Slider {
                        label = "Wind Direction",
                        getValue = new Func<ClampedFloatParameter>(() => { return _settings.direction; }),
                        unit = "°",
                        setValue = new Action<float>((float f) => { _settings.direction.value = f; })
                    }
                },
                {
                    "wind_direction_variance",
                    new Slider {
                        label = "Wind Direction Variance",
                        getValue = new Func<ClampedFloatParameter>(() => { return new ClampedFloatParameter(percentageClamped(_settings.directionVariance), 0f, 100f); }),
                        unit = "%",
                        setValue = new Action<float>((float f) => { _settings.directionVariance.value = setClampedValuePercent(_settings.directionVariance, f); })
                    }
                },
                {
                    "wind_direction_variance_period",
                    new Slider {
                        label = "Wind Direction Variance Period",
                        getValue = new Func<ClampedFloatParameter>(() => { return _settings.directionVariancePeriod; }),
                        unit = "s",
                        setValue = new Action<float>((float f) => { _settings.directionVariancePeriod.value = f; })
                    }
                }
            };
            return fields;

        }

        private void SetBoolValue(string key, bool value) {
            // TODO Make this more safe
            var field = (Checkbox)fields.Get(key);
            field.setChecked(value);
        }

        private void SetFloatValue(string key, float value) {
            // TODO Make this more safe
            var field = (Slider)fields.Get(key);
            field.setValue(value);
        }

        private float percentageClamped(ClampedFloatParameter cfp) {
            return 100f * (cfp.value - cfp.min) / (cfp.max - cfp.min);
        }

        private float setClampedValuePercent(ClampedFloatParameter cfp, float percent) {
            return cfp.min + percent / 100f * (cfp.max - cfp.min);
        }
    }


}
