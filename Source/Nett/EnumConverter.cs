﻿namespace Nett
{
    using System.Reflection;
    using System;

    internal sealed class EnumToTomlConverter : ITomlConverter
    {
        public Type FromType => Types.EnumType;

        public bool CanConvertFrom(Type t) => t.GetTypeInfo().BaseType == Types.EnumType;

        public bool CanConvertTo(Type t) => Types.TomlObjectType.IsAssignableFrom(t);

        public bool CanConvertToToml() => true;

        public object Convert(ITomlRoot root, object value, Type targetType) => new TomlString(root, value.ToString());
    }

    internal sealed class TomlToEnumConverter : ITomlConverter
    {
        public Type FromType => Types.TomlStringType;

        public bool CanConvertFrom(Type t) => t == this.FromType;

        public bool CanConvertTo(Type t) => t.GetTypeInfo().BaseType == Types.EnumType;

        public bool CanConvertToToml() => false;

        public object Convert(ITomlRoot root, object value, Type targetType) => Enum.Parse(targetType,
            ((TomlString) value).Value);
    }
}