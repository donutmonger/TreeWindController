using System;
using System.Collections.Generic;
using System.Text;
using Colossal.UI.Binding;

namespace TreeWindController.UI {
    public class Checkbox : IJsonWritable {
        public string label;
        public Func<bool> getChecked;
        public Action<bool> setChecked;

        public void Write(IJsonWriter writer) {
            writer.TypeBegin(this.GetType().FullName);

            writer.PropertyName("label");
            writer.Write(label);

            writer.PropertyName("checkedValue");
            writer.Write(getChecked());

            writer.TypeEnd();
        }
    }
}
