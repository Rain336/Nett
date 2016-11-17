using Nett.Coma;
using Nett.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nett.Ergo
{
    internal sealed class SourceContext : IDisposable
    {
        private static ThreadLocal<SourceContext> CurrentContext { get; } = new ThreadLocal<SourceContext>(() => null);

        public static bool IsSourceActive(out IConfigSource source)
        {
            source = CurrentContext.Value?.Source;
            return source != null;
        }

        public IConfigSource Source { get; }

        private SourceContext(IConfigSource source)
        {
            this.Source = source.CheckNotNull(nameof(source));
        }

        public static SourceContext Create(IConfigSource source)
        {
            var sc = new SourceContext(source);

            CurrentContext.Value = sc;
            return sc;
        }

        public void Dispose() => CurrentContext.Value = null;
    }
}
