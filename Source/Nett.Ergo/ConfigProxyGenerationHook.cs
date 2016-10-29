using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Nett.Ergo
{
    internal sealed class ConfigProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            throw new Exception($"Member '{memberInfo.DeclaringType.Name}.{memberInfo.Name}' cannot be proxied.");
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return true;
        }
    }
}
