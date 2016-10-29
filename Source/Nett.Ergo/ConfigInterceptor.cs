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
        private static readonly object NotSupported = new object();
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        private readonly TPath parent = new TPath();
        private readonly Config<T> config;
        private readonly Dictionary<MethodInfo, object> subProxies = new Dictionary<MethodInfo, object>();

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
            object subProxy;
            if (this.subProxies.TryGetValue(invocation.Method, out subProxy))
            {
                invocation.ReturnValue = subProxy;
                return;
            }
            else
            {
                if (IsSetter(invocation))
                {
                    this.HandleSetter(invocation);
                }
                else if (IsGetter(invocation))
                {

                }
            }
        }

        private string GetPropertyName(IInvocation invocation) => invocation.Method.Name.Substring("_et_".Length);

        private void HandleSetter(IInvocation invocation)
        {
            var propertyName = GetPropertyName(invocation);
            var path = this.parent.WithKeyAdded(propertyName);
            this.config.Untyped.Set(path, invocation.Arguments[0]);
        }

        private void HandleGetter(IInterceptor invocation)
        {

        }

        private bool IsSetter(IInvocation invocation) => invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
        private bool IsGetter(IInvocation invocation) => invocation.Method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase);

        private void InitSubProxies(Type t)
        {
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties.Where(p => NeedsSubProxy(p)))
            {
                var getter = p.GetGetMethod(nonPublic: false);
                this.subProxies[getter] = CreateSubProxy(p);

                var setter = p.GetSetMethod(nonPublic: false);
                this.subProxies[setter] = NotSupported;
            }
        }

        private object CreateSubProxy(PropertyInfo pi)
        {
            var interceptor = new ConfigInterceptor<T>(this.config, pi.PropertyType, this.parent.WithKeyAdded(pi.Name));
            var options = new ProxyGenerationOptions() { Hook = new ConfigProxyGenerationHook() };
            var proxy = generator.CreateClassProxy(pi.PropertyType, options, interceptor);
            return proxy;
        }

        private static bool NeedsSubProxy(PropertyInfo pi) => !pi.PropertyType.IsValueType && pi.PropertyType != typeof(string);
    }
}
