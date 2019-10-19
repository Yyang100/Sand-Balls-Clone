using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate region with width and height parameters
/// 
/// </summary>
public class RegionGenerator : MonoBehaviour
{
    /// <summary>
    /// Scale of map (pieces)
    /// </summary>
    public float scale = 1f;
    /// <summary>
    /// Width of map
    /// </summary>
    public int width;
    /// <summary>
    /// Height of map
    /// </summary>
    public int height;

    [SerializeField]
    private bool randomObstacle;  

    /// <summary>
    /// Map parameters
    /// </summary>
    private float[,] map;
      
    /// <summary>
    /// Mesh generator
    /// </summary>
    private MeshGenerator meshGenerator;

    private Vector3 offset;

    /// <summary>
    /// Get Mesh Generator and calculate offset
    /// </summary>
    private void Awake()
    {
        meshGenerator = GetComponent<MeshGenerator>();

        offset = new Vector3((float)width * scale / 2f + scale / 2f, (float)height * scale / 2f + scale / 2f, 0f) - Vector3.one * scale;
    }

    private void Start()
    {
        InialitizeMap();
    }

    public void InialitizeMap()
    {
        GenerateMap();
        if (randomObstacle)
        { 
            SpawnObstacles();
        }
        CutStamps();
    }

    private void GenerateMap()
    {
        map = new float[width, height];
        FillMap();
        meshGenerator.GenerateMesh(map, scale);
    }
     
    /// <summary>
    /// make edges 0, others 1
    /// </summary>
    private void FillMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    map[i, j] = 0f;
                }
                else
                {
                    map[i, j] = 1f;
                }
            }
        }
    }

    /// <summary>
    /// Spawn random obstacle
    /// </summary>
    private void SpawnObstacles()
    {
        GameObject obstacle = ObstacleManager.Instance.GetRandomObstacleSet();
        obstacle.transform.position = transform.position;
        obstacle.transform.parent = transform; 
    }

    /// <summary>
    /// Create holls/stamps
    /// Search the stapms in children
    /// </summary>
    private void CutStamps() ///
    {
        Stamp[] componentsInChildren = GetComponentsInChildren<Stamp>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].CutStamp(this);
        }
    }

    /// <summary>
    /// Cut the map using position and radius
    /// Called after gettin mouse position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    public void Cut(Vector3 position, float radius)
    {
        position += offset - transform.position;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 v = new Vector3((float)i, (float)j) * scale;
                float num = Vector2.Distance(position, v);
                if (num < radius)
                {
                    float num2 = num / radius;
                    if (num2 < map[i, j])
                    {
                        map[i, j] = num2;
                    }
                }
            }
        }
        meshGenerator.GenerateMesh(map, scale);
    }

    /// <summary>
    /// Draw a wire cube to see map area
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3((float)(width - 2) * scale, (float)(height - 2) * scale - 0.32f, 0.01f));
    }

   
}
