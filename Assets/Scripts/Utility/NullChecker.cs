using UnityEngine;

public abstract class NullChecker
{
    public static bool verbose = false;

    public static bool Check(Object obj)
    {
        string objectType = obj != null ? obj.GetType().Name : "null";

        if (obj == null)
        {
            Debug.Log($"{objectType} is null");
            return true;
        }
        else
        {
            if(verbose == true)
                Debug.Log($"{objectType} is not null");
            return false;
        }
    }

    public static bool Check(GameObject gameObject)
    {
        return Check(gameObject as Object);
    }

    public static bool Check(Component component)
    {
        return Check(component as Object);
    }
}
