using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nett
{
    internal sealed class TomlTableToDictionaryConverter : ITomlConverter
    {
        private static readonly Type DictType = typeof(Dictionary<string, object>);

        public Type FromType => Types.TomlTableType;

        public bool CanConvertFrom(Type t) => t == Types.TomlTableType;

        public bool CanConvertTo(Type t) => DictType.IsAssignableFrom(t);

        public bool CanConvertToToml() => false;

        public object Convert(ITomlRoot root, object value, Type targetType) => Convert((TomlTable)value);

        internal static Dictionary<string, object> Convert(TomlTable table)
        {
            Dictionary<string, object> converted = new Dictionary<string, object>(table.Count);

            foreach (var r in table.Rows)
            {
                Type tableToTypeMappingTargetType;
                if (r.Value.TomlType == TomlObjectType.Table
                    && (tableToTypeMappingTargetType = table.Root.Config.TryGetMappedType(r.Key, null)) != null)
                {
                    converted[r.Key] = table.Get(tableToTypeMappingTargetType);
                }
                else
                {
                    converted.Add(r.Key, ConvertItem(r.Value));
                }
            }

            return converted;
        }

        internal static object ConvertItem(TomlObject to)
        {
            switch (to.TomlType)
            {
                case TomlObjectType.Int: return ((TomlInt)to).Value;
                case TomlObjectType.Bool: return ((TomlBool)to).Value;
                case TomlObjectType.Float: return ((TomlFloat)to).Value;
                case TomlObjectType.String: return ((TomlString)to).Value;
                case TomlObjectType.TimeSpan: return ((TomlTimeSpan)to).Value;
                case TomlObjectType.DateTime: return ((TomlDateTime)to).Value;
                case TomlObjectType.Array: return ConvertArray((TomlArray)to);
                case TomlObjectType.Table: return Convert((TomlTable)to);
                case TomlObjectType.ArrayOfTables: return ConvertTableArray((TomlTableArray)to);
                default: return null;
            }
        }

        private static object ConvertArray(TomlArray array) => array.Items.Select(i => ConvertItem(i)).ToArray();

        private static object ConvertTableArray(TomlTableArray to) => to.Items.Select(i => Convert(i)).ToArray();
    }
}
