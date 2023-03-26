using System;
using System.Collections.Generic;


namespace DependencyManager
{
    public static class DependencyManager
    {
        private static readonly Dictionary<Type, object> InstanceFactory = new Dictionary<Type, object>();
        
        public static void CreateNewInstance<T>(T instance)where T : IDisposable
        {
            InstanceFactory.Add(typeof(T), instance);
        }

        public static void DestroyInstance<T>() where T : IDisposable
        {
            ((T)InstanceFactory[typeof(T)]).Dispose();
            InstanceFactory.Remove(typeof(T));
        }

        public static T GetInstance<T>() where T : IDisposable
        {
            return (T)InstanceFactory[typeof(T)];
        }
    }
}