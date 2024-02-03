using System;
using System.Collections.Generic;
using System.Text;
using Game.UI;
using Colossal.Annotations;
using Colossal.Logging;
using Colossal.UI.Binding;
using UnityEngine.Rendering;
using UnityEngine;

namespace TreeWindController.Systems {

    public class Checkbox: IJsonWritable {
        public string label;
        public bool checkedValue;

        public void Write(IJsonWriter writer) {
            writer.TypeBegin(this.GetType().FullName);

            writer.PropertyName("label");
            writer.Write(label);

            writer.PropertyName("checkedValue");
            writer.Write(checkedValue);

            writer.TypeEnd();
        }
    }

    public class Slider : IJsonWritable {
        public string label;
        public ClampedFloatParameter value;
        public bool percentage;
        public string unit;

        public void Write(IJsonWriter writer) {
            writer.TypeBegin(this.GetType().FullName);

            writer.PropertyName("label");
            writer.Write(label);

            // TODO can ClampedFloatParameter just implement this itself?
            writer.PropertyName("min");
            writer.Write(value.min);

            writer.PropertyName("max");
            writer.Write(value.max);

            writer.PropertyName("value");
            writer.Write(value.value);

            writer.PropertyName("percentage");
            writer.Write(percentage);

            writer.PropertyName("unit");
            writer.Write(unit);

            writer.TypeEnd();
        }
    }

    class SettingsUISystem : UISystemBase {
        private ILog _log;
        private string kGroup = "tree-wind-controller";

        protected override void OnCreate() {
            base.OnCreate();

            _log = Mod.Instance.Log;
            _log.Info("SettingsUISystem.OnCreate");


            this.AddUpdateBinding(
                new GetterValueBinding<Dictionary<string,IJsonWritable>>(this.kGroup, "get_values", GetValues,
                new MyDictionaryWriter<string, IJsonWritable>())
            );

            this.AddBinding(new TriggerBinding<string,bool>(kGroup, "set_bool_value", new Action<string,bool>(SetBoolValue)));
            this.AddBinding(new TriggerBinding<string,float>(kGroup, "set_value", new Action<string,float>(SetValue)));
        }

        private Dictionary<string,IJsonWritable> GetValues() {
            var fields = new Dictionary<string, IJsonWritable>();
            fields.Add(
                "disable_wind", 
                new Checkbox {
                    label = "Disable All Wind",
                    checkedValue = SettingsSystem.Instance.disableAllWind,
                }
            );
            fields.Add(
                "wind_strength", 
                new Slider {
                    label = "Wind Strength", 
                    value = new ClampedFloatParameter(percentageClamped(SettingsSystem.Instance.strength), 0f, 100f), 
                    percentage = true, 
                    unit = "%"
                }
            );
            fields.Add(
                "wind_strength_variance_period", 
                new Slider {
                    label = "Wind Strength Variance Period", 
                    value = SettingsSystem.Instance.strengthVariancePeriod,
                    percentage = true, 
                    unit = "s"
                }
            );
            fields.Add(
                "wind_direction", 
                new Slider {
                    label = "Wind Direction", 
                    value = SettingsSystem.Instance.direction, 
                    percentage = false, 
                    unit = "°"
                }
            );
            fields.Add(
                "wind_direction_variance", 
                new Slider {
                    label = "Wind Direction Variance", 
                    value = new ClampedFloatParameter(percentageClamped(SettingsSystem.Instance.directionVariance), 0f, 100f), 
                    percentage = true, 
                    unit = "%",
                }
            );

            fields.Add(
                "wind_direction_variance_period", 
                new Slider {
                    label = "Wind Direction Variance Period", 
                    value = SettingsSystem.Instance.directionVariancePeriod,
                    percentage = false, 
                    unit = "s",
                }
            );
            return fields;

        }

        private void SetBoolValue(string key, bool value) {
            switch (key) {
                case "disable_wind":
                    SettingsSystem.Instance.disableAllWind = value;
                    break;
            }
        }

        private void SetValue(string key, float value) {
            switch (key) {
                case "wind_strength":
                    SettingsSystem.Instance.strength.value = setClampedValuePercent(SettingsSystem.Instance.strength, value);
                    break;

                case "wind_strength_variance_period":
                    SettingsSystem.Instance.strengthVariancePeriod.value = value;
                    break;

                case "wind_direction":
                    SettingsSystem.Instance.direction.value = value;
                    break;

                case "wind_direction_variance":
                    SettingsSystem.Instance.directionVariance.value = setClampedValuePercent(SettingsSystem.Instance.directionVariance, value);
                    break;


                case "wind_direction_variance_period":
                    SettingsSystem.Instance.directionVariancePeriod.value = value;
                    break;

            }
        }

        private float percentageClamped(ClampedFloatParameter cfp) {
            return 100f * (cfp.value - cfp.min) / (cfp.max - cfp.min);
        }

        private float setClampedValuePercent(ClampedFloatParameter cfp, float percent) {
            return cfp.min + percent / 100f * (cfp.max - cfp.min);
        }
    }

    public class MyDictionaryWriter<K, V> : IWriter<IDictionary<K, V>> {
        [NotNull]
        private readonly IWriter<K> m_KeyWriter;

        [NotNull]
        private readonly IWriter<V> m_ValueWriter;

        public MyDictionaryWriter(IWriter<K> keyWriter = null, IWriter<V> valueWriter = null) {
            m_KeyWriter = keyWriter ?? ValueWriters.Create<K>();
            m_ValueWriter = valueWriter ?? ValueWriters.Create<V>();
        }

        public void Write(IJsonWriter writer, IDictionary<K, V> value) {
            if (value != null) {
                writer.MapBegin(value.Count);
                foreach (KeyValuePair<K, V> item in value) {
                    m_KeyWriter.Write(writer, item.Key);

                    if (item.Value is IDictionary<K, V> nestedDictionary) {
                        Write(writer, nestedDictionary);
                    } else {
                        m_ValueWriter.Write(writer, item.Value);
                    }
                }
                writer.MapEnd();
                return;
            }

            writer.WriteNull();
            throw new ArgumentNullException("value", "Null passed to non-nullable dictionary writer");
        }
    }
}
