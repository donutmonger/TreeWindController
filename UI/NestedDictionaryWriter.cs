using System;
using System.Collections.Generic;
using System.Text;
using Colossal.UI.Binding;
using Colossal.Annotations;

namespace TreeWindController.UI {
    public class NestedDictionaryWriter<K, V> : IWriter<IDictionary<K, V>> {
        [NotNull]
        private readonly IWriter<K> m_KeyWriter;

        [NotNull]
        private readonly IWriter<V> m_ValueWriter;

        public NestedDictionaryWriter(IWriter<K> keyWriter = null, IWriter<V> valueWriter = null) {
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
