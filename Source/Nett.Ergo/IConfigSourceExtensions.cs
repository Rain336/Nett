using Nett.Coma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nett.Ergo
{
    public static class IConfigSourceExtensions
    {
        public static IDisposable MakeCurrent(this IConfigSource source) => SourceContext.Create(source);
    }
}
