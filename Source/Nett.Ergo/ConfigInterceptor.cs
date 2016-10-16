using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Nett.Coma;

namespace Nett.Ergo
{
    internal sealed class ConfigInterceptor<T> : IInterceptor
        where T : class
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        private readonly TPath parent = new TPath();
        private readonly Config<T> config;
        private readonly Dictionary<MethodInfo, Func<object>> subProxies = new Dictionary<MethodInfo, Func<object>>();

        public ConfigInterceptor(Config<T> config)
            : this(config, typeof(T), new TPath())
        {
            this.config = config;
        }

        public ConfigInterceptor(Config<T> config, Type proxyType, TPath parent)
        {
            this.config = config;
            this.parent = parent;
            InitSubProxies(proxyType);
        }

        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        private void InitSubProxies(Type t)
        {
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties.Where(p => NeedsSubProxy(p)))
            {
                var getter = p.GetGetMethod(nonPublic: false);
                this.subProxies[getter] = CreateSubProxy(p);

                var setter = p.GetSetMethod(nonPublic: false);
                this.subProxies[setter] = () => { throw new NotSupportedException(); };
            }
        }

        private Func<object> CreateSubProxy(PropertyInfo pi)
        {
            var interceptor = new ConfigInterceptor<T>(this.config, pi.PropertyType, this.parent.WithKeyAdded(pi.Name));
            var options = new ProxyGenerationOptions() { Hook = new ConfigProxyGenerationHook() };
            var proxy = generator.CreateClassProxy(pi.PropertyType, options, interceptor);
            return () => proxy;
        }

        private static bool NeedsSubProxy(PropertyInfo pi) => !pi.PropertyType.IsValueType;
    }
}
