using System;
using System.Collections.Generic;
using System.Text;
using Colossal.UI.Binding;

namespace TreeWindController.UI {
    public class DictionaryEqualityComparer : EqualityComparer<Dictionary<string, IJsonWritable>> {
        public override bool Equals(Dictionary<string, IJsonWritable> x, Dictionary<string, IJsonWritable> y) {
            // TODO we can do better!
            return false;
        }

        public override int GetHashCode(Dictionary<string, IJsonWritable> obj) {
            return obj.GetHashCode();
        }
    }
}
