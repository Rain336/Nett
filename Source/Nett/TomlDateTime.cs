﻿using System;

namespace Nett
{
    public sealed class TomlDateTime : TomlValue<DateTimeOffset>
    {
        public override string ReadableTypeName => "date time";

        public TomlDateTime(DateTimeOffset value)
            : base(value)
        {

        }

        public override void Visit(ITomlObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
