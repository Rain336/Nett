using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Nett.TestDecoder
{
    public class ToJsonConverter
    {
        private StringBuilder sb = new StringBuilder();

        public static string Convert(TomlTable table)
        {
            StringBuilder sb = new StringBuilder();
            ConvertTable(sb, table);
            return sb.ToString();
        }

        private static void ConvertTable(StringBuilder sb, TomlTable table)
        {
            sb.Append("{");

            foreach (var r in table.Rows)
            {
                sb.Append("\"").Append(r.Key).Append("\":");
                ConvertItem(sb, r.Value);
                sb.Append(",");
            }

            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);

            sb.Append("}");
        }

        private static void ConvertItem(StringBuilder sb, TomlObject item)
        {
            switch(item.TomlType)
            {
                case TomlObjectType.Bool:
                    TomlBool b = (TomlBool)item;
                    sb.Append("{").Append("\"type\":\"bool\", \"value\":\"").Append(b.Value ? "true" : "false").Append("\"}");
                    break;
                case TomlObjectType.Int:
                    TomlInt i = (TomlInt)item;
                    sb.Append("{").Append("\"type\":\"integer\", \"value\":\"").Append(i.Value).Append("\"}");
                    break;
                case TomlObjectType.TimeSpan:
                    TomlTimeSpan ts = (TomlTimeSpan)item;
                    sb.Append("{").Append("\"type\":\"timespan\", \"value\":\"").Append(ts.Value).Append("\"}");
                    break;
                case TomlObjectType.Array:
                    TomlArray array = (TomlArray)item;
                    ConvertArray(sb, array);
                    break;
                case TomlObjectType.DateTime:
                    TomlDateTime dt = (TomlDateTime)item;
                    sb.Append("{").Append("\"type\":\"datetime\", \"value\":\"").Append(XmlConvert.ToString(dt.Value.UtcDateTime, XmlDateTimeSerializationMode.Utc)).Append("\"}");
                    break;
                case TomlObjectType.String:
                    TomlString s = (TomlString)item;
                    sb.Append("{").Append("\"type\":\"string\", \"value\":\"").Append(s.Value.Replace(Environment.NewLine, "\n").Escape()).Append("\"}");
                    break;
                case TomlObjectType.Float:
                    TomlFloat f = (TomlFloat)item;
                    sb.Append("{").Append("\"type\":\"float\", \"value\":\"").Append(f.Value.ToString(CultureInfo.InvariantCulture)).Append("\"}");
                    break;
                case TomlObjectType.ArrayOfTables:
                    TomlTableArray ta = (TomlTableArray)item;
                    ConvertTomlTableArray(sb, ta);
                    break;
            }
        }

        private static void ConvertArray(StringBuilder sb, TomlArray a)
        {
            sb.Append("{").Append("\"type\":\"array\", \"value\":[");

            for (int i = 0; i < a.Length - 1; i++)
            {
                ConvertItem(sb, a[i]);
                sb.Append(",");
            }

            if (a.Length > 0) { ConvertItem(sb, a[a.Length - 1]); }

            sb.Append("]}");
        }

        private static void ConvertTomlTableArray(StringBuilder sb, TomlTableArray a)
        {
            sb.Append("[");

            for (int i = 0; i < a.Count - 1; i++)
            {
                ConvertTable(sb, a[i]);
                sb.Append(",");
            }

            if (a.Count > 0) { ConvertTable(sb, a[a.Count - 1]); }

            sb.Append("]");
        }
    }
}
