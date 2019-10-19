using System;
using UnityEngine;
 
    [Serializable]
    public class PoolableObject
    {     
        public GameObject ItemPrefab; 
        public int PoolSize;
        public Transform parent;

        public PoolableObject(GameObject itemPrefab, int poolSize,Transform parent)
        {
            this.ItemPrefab = itemPrefab;
            this.PoolSize = poolSize;
            this.parent = parent;
        }

    
    } 