using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Nett.Coma;

namespace Nett.Ergo
{
    internal sealed class ConfigInterceptor<T> : IInterceptor
        where T : class
    {
        private Config<T> config;

        public ConfigInterceptor(Config<T> config)
        {
            this.config = config;
        }

        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
