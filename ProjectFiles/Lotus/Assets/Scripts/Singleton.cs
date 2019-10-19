using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create Singelton objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            var instances = FindObjectsOfType<T>();
            var count = instances.Length;
            if (count > 0)
            {
                if (count == 1)
                    return _instance = instances[0];
                for (var i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);
                return _instance = instances[0];
            }

            return _instance = new GameObject(typeof(T).Name).AddComponent<T>();
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
