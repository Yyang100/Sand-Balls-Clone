using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject; 

public class PoolManager : MonoBehaviour, IPoolManager
{   
    #region Public Variables

    public bool _activeAtStart = false;
    public Dictionary<GameObject, Queue<GameObject>> _awailableGameObjectsDict = new Dictionary<GameObject, Queue<GameObject>>();

    #endregion


    #region Private Variables

    private const int DefaultPoolSize = 1;  
    private Dictionary<GameObject, GameObject> _busyGameObjectsDict = new Dictionary<GameObject, GameObject>();

    #endregion
    

    public void Pool(PoolableObject poolableObject)
    { 
        CreatePool(poolableObject.ItemPrefab, poolableObject.PoolSize,poolableObject.parent); 
    }

    public void ExpandPool(GameObject prefab, int poolSize, Transform parent)
    {
        CreatePool(prefab, poolSize, parent);
    }
    
    private void CreatePool(GameObject prefab, int poolSize,Transform parent)
    {
        if (!_awailableGameObjectsDict.ContainsKey(prefab))
        {
            _awailableGameObjectsDict.Add(prefab, new Queue<GameObject>());
        }
        for (int i = 0; i < poolSize; i++)
        {
            GameObject gameObject = Instantiate<GameObject>(prefab, parent);
            gameObject.SetActive(_activeAtStart); 
            _awailableGameObjectsDict[prefab].Enqueue(gameObject);
        }
         
    }
     

    #region Spawn methods
    public GameObject Spawn(GameObject prefab)
    {
        return Spawn(prefab, null, Vector3.zero, prefab.transform.rotation, prefab.transform.localScale);
    }

    public GameObject Spawn(GameObject prefab, Transform parent)
    {
        return Spawn(prefab, parent, Vector3.zero, prefab.transform.rotation, prefab.transform.localScale);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        return Spawn(prefab, null, position, prefab.transform.rotation, prefab.transform.localScale);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Vector3 scale)
    {
        return Spawn(prefab, null, position, prefab.transform.rotation, scale);
    }

    public GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
    {
        return Spawn(prefab, parent, position, prefab.transform.rotation, prefab.transform.localScale);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Spawn(prefab, null, position, rotation, prefab.transform.localScale);
    }
    #endregion

    /// <summary>
    /// Recyle the gameObject
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(GameObject obj)
    {
        if (obj == null)
        {
            return;
        } 
        obj.SetActive(false);
        if (_busyGameObjectsDict.ContainsKey(obj))
        {
            GameObject key = _busyGameObjectsDict[obj];
            if (_busyGameObjectsDict.Remove(obj))
            {
                _awailableGameObjectsDict[key].Enqueue(obj);
            }
        }
    }

    public GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (!_awailableGameObjectsDict.ContainsKey(prefab))
        {
            CreatePool(prefab, DefaultPoolSize,parent);
        }
        if (_awailableGameObjectsDict[prefab].Count == 0)
        {
            CreatePool(prefab, DefaultPoolSize,parent);
        }
        GameObject gameObject = _awailableGameObjectsDict[prefab].Dequeue();
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
        gameObject.SetActive(true);
        _busyGameObjectsDict.Add(gameObject, prefab);
        return gameObject;
    }

        
} 