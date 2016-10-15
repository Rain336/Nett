﻿using System;
using Nett.Extensions;

namespace Nett.Coma
{
    internal sealed class TomlSource : TomlObject
    {
        public TomlSource(ITomlRoot root, IConfigSource source)
            : base(root)
        {
            this.Value = source;
        }

        public IConfigSource Value { get; }

        public override string ReadableTypeName => "Value source";

        // Workaround for the 'internal TOML type' TomlSource as the type doesn't matter for this internal type
        // How can we provide a better value for this enum inside the COMA lib?
        public override TomlObjectType TomlType => TomlObjectType.Int;

        public override object Get(Type t) => this.Value;

        internal override TomlObject WithRoot(ITomlRoot root)
        {
            root.CheckNotNull(nameof(root));

            return new TomlSource(root, this.Value);
        }
    }
}
