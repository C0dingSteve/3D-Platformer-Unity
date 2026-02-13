using UnityEngine;

namespace Assets.Scripts.Utility
{
    public static class NullChecker
    {
        public static T IsNull<T>(this T obj) where T : Object => obj ? obj : null;
        public static void LogNullStatus<T>(this T obj) where T : Object
        {
            if (obj == null) Debug.Log($"Object of type {typeof(T).FullName} is null");
            //else Debug.Log($"{obj} is not null");
        }
    }
}