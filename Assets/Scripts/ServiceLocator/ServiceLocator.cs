using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly IDictionary<Type, object> GlobalServices = new Dictionary<Type, object>();
        private static readonly IDictionary<Type, object> LocalServices = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            var targetDict = GetTargetDictionary(typeof(T));
            
            if(targetDict.TryGetValue(typeof(T), out object existing))
            {
                if(ReferenceEquals(existing, service)) return;    
                if(existing is UnityEngine.Object unityObj && unityObj != null)
                {   
                    Debug.LogWarning($"Service of type {typeof(T)} already exists with a different instance!");
                    return;
                }
            }
            targetDict[typeof(T)] = service;
        }

        public static bool TryGet<T>(out T service)
        {
            Type type = typeof(T);

            // Check Local first, then Global
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

        public static T Get<T>() where T : Component
        {
            // Check if the service is already cached in our dictionaries
            if (TryGet(out T service) && service != null) return service;
            
            // Search the hierarchy for instances manually placed in the scene
            service = UnityEngine.Object.FindFirstObjectByType<T>();
            if (service != null) 
            {
                Register(service);
                return service;
            }

            // If no services found yet, create a new GameObject to host the Service
            GameObject container = new($"[Service] - {typeof(T).Name}");
            // Add the component. Note: AddComponent triggers Awake() immediately
            service = container.AddComponent<T>();

            // Safety check: The component's Awake() method may self-register 
            // So, we check for existing service once again
            if (TryGet(out T existingService) && existingService != null) return existingService;

            // If nothing was registerd in the component's awake call 
            // we register the newly created service and return it
            Register(service);
            return service;
        }

        public static void Unregister<T>(bool isGlobal = false)
        {
            GetTargetDictionary(typeof(T)).Remove(typeof(T));
        }

        public static void ClearLocalServices() => LocalServices.Clear();

        private static IDictionary<Type, object> GetTargetDictionary(Type type)
        {
            if(typeof(IGlobalService).IsAssignableFrom(type))
            {
                return GlobalServices;
            }
            return LocalServices;
        }
    }
}