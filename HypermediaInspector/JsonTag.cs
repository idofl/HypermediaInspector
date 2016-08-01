using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypermediaInspector
{
    public class JsonTag
    {
        public JsonTag(JsonReader reader)
        {
            TokenType = reader.TokenType;
            Value = reader.Value;
            ValueType = reader.ValueType;
        }

        public JsonToken TokenType { get; set; }
        public object Value { get; set; }
        public Type ValueType { get; set; }
    }

}
