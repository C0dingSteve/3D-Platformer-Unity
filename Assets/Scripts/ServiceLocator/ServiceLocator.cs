using System;
using System.Collections.Generic;

namespace Assets.Scripts.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly IDictionary<Type, object> GlobalServices = new Dictionary<Type, object>();
        private static readonly IDictionary<Type, object> LocalServices = new Dictionary<Type, object>();

        public static void Register<T>(T service, bool isGlobal = false)
        {
            var targetDict = isGlobal ? GlobalServices:LocalServices;
            
            if(targetDict.ContainsKey(typeof(T)))
                throw new ArgumentException($"Service of type {typeof(T)} already exists");

            targetDict[typeof(T)] = service;
        }

        public static bool TryGet<T>(out T service)
        {
            Type type = typeof(T);
            if(LocalServices.TryGetValue(type, out object localService))
            {
                service = (T)localService;
                return true;
            }
            if(GlobalServices.TryGetValue(type, out object globalService))
            {
                service = (T)globalService;
                return true;
            }
            service = default;
            return false;
        }

        public static T Get<T>()
        {
            if(TryGet(out T service)) return service;
            throw new ArgumentException($"Service of type {typeof(T)} not found");
        }

        public static void Unregister<T>(bool isGlobal = false)
        {
            var target = isGlobal ? GlobalServices:LocalServices;
            target.Remove(typeof(T));
        }

        public static void ClearLocalServices() => LocalServices.Clear();
    }
}