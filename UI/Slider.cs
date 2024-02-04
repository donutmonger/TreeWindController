using System;
using System.Collections.Generic;
using System.Text;
using Colossal.UI.Binding;
using UnityEngine.Rendering;

namespace TreeWindController.UI {
    public class Slider : IJsonWritable {
        public string label;
        public string unit;

        public Func<ClampedFloatParameter> getValue;
        public Action<float> setValue;

        public void Write(IJsonWriter writer) {
            writer.TypeBegin(this.GetType().FullName);

            writer.PropertyName("label");
            writer.Write(label);

            // TODO can ClampedFloatParameter just implement this itself?
            var value = getValue();

            writer.PropertyName("min");
            writer.Write(value.min);

            writer.PropertyName("max");
            writer.Write(value.max);

            writer.PropertyName("value");
            writer.Write(value.value);

            writer.PropertyName("unit");
            writer.Write(unit);

            writer.TypeEnd();
        }
    }
}
