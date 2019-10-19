using System;
using System.Collections.Generic;
using UnityEngine;
    public interface IPoolManager 
    { 
        GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale);

        GameObject Spawn(GameObject prefab);

        GameObject Spawn(GameObject prefab, Transform parent);

        GameObject Spawn(GameObject prefab, Vector3 position);

        GameObject Spawn(GameObject prefab, Vector3 position, Vector3 scale);

        GameObject Spawn(GameObject prefab, Transform parent, Vector3 position);

        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);

        void Recycle(GameObject obj);
    }
