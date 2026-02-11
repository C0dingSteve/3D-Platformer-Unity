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
            
            if(!targetDict.ContainsKey(typeof(T)))
                throw new ArgumentException($"Service of type {typeof(T)} already exists");

            targetDict[typeof(T)] = service;
        }

        public static T Get<T>()
        {
            if(LocalServices.TryGetValue(typeof(T), out object localService)) return (T) localService;
            if(GlobalServices.TryGetValue(typeof(T), out object globalService)) return (T) globalService;

            throw new ArgumentException($"Service of type {typeof(T)} not found");
        }

        public static void ClearLocalServices() => LocalServices.Clear();
    }
}