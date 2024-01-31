using System;
using System.Collections.Generic;
using System.Text;
using Game.UI;
using Colossal.Annotations;
using Colossal.Logging;
using Colossal.UI.Binding;
using UnityEngine.Rendering;

namespace TreeWindController.Systems {

    class SettingsUISystem : UISystemBase {
        private ILog _log;
        private string kGroup = "tree-wind-controller";


        protected override void OnCreate() {
            base.OnCreate();

            _log = Mod.Instance.Log;
            _log.Info("SettingsUISystem.OnCreate");

            this.AddUpdateBinding(
                new GetterValueBinding<Dictionary<string,float>>(this.kGroup, "get_values", () => {
                    return new Dictionary<string, float>() {
                        {"wind_strength", percentageClamped(SettingsSystem.Instance.strength) },
                        {"wind_direction", SettingsSystem.Instance.direction.value }
                    };
                },
                new MyDictionaryWriter<string, float>())
            );

            this.AddBinding(new TriggerBinding<string,float>(kGroup, "set_value", new Action<string,float>(SetValue)));
        }


        private void SetValue(string key, float value) {
            switch (key) {
                case "wind_strength":
                    SettingsSystem.Instance.strength.value = setClampedValuePercent(SettingsSystem.Instance.strength, value);
                    break;
                case "wind_direction":
                    SettingsSystem.Instance.direction.value = value;
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
