//	Copyright (c) Kimpuppy.
//	http://bakak112.tistory.com/

using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                instance = new GameObject("@" + typeof(T).ToString(),
                      typeof(T)).GetComponent<T>();
            }
            return instance;
        }
    }
}
