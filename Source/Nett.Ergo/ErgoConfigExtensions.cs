using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Nett.Coma;

namespace Nett.Ergo
{
    public static class ErgoConfigExtensions
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public static T CreateErgoProxy<T>(this Config<T> config)
            where T : class
        {
            var configInterceptor = new ConfigInterceptor<T>(config);
            var proxy = generator.CreateClassProxy<T>(configInterceptor);
            return proxy;
        }
    }
}
