using System;
using System.Collections.Generic;

namespace Assets.Scripts.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly IDictionary<Type, object> Services = new Dictionary<Type, object>();

        public static void RegisterService<T>(T service)
        {
            try
            {
                if(!Services.ContainsKey(typeof(T)))
                {
                    Services[typeof(T)] = service;
                }
            }
            catch
            {
                throw new ArgumentException($"Service of type {typeof(T)} already exists");
            }
        }

        public static T GetService<T>()
        {
            try
            {
                return (T) Services[typeof(T)];
            }
            catch
            {
                throw new ArgumentException($"Service of type {typeof(T)} not found");
            }
        }
    }
}