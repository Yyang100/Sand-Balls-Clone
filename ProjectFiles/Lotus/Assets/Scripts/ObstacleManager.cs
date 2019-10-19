using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the obtacles
/// </summary>
public class ObstacleManager : Singleton<ObstacleManager>
{
    public List<GameObject> obstacleList;

     /// <summary>
     /// Get a obstacle //randomly
     /// not instantiate, change the positions
     /// </summary>
     /// <returns></returns>
    public GameObject GetRandomObstacleSet()
    {
        int i = Random.Range(0, obstacleList.Count);
        obstacleList[i].SetActive(true);
        return obstacleList[i];
    }

    /// <summary>
    /// Reset the obstacles
    /// set active false
    /// </summary>
    public void ResetObstacles()
    {
        foreach (GameObject g in obstacleList)
        {
            g.transform.parent = transform;
            g.SetActive(false);
        }
    }

}
