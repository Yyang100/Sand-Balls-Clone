using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

/// <summary>
/// Manage the floors?
/// Set their positions 
/// </summary>
public class FloorManager : MonoBehaviour
{
    #region D.I.
    private GameManager _gameManager;
    private PoolManager _poolManager;
    #endregion

    #region publicVariables
    public int _floorByLevel = 3;
    public float _floorSize = 5.5f;
    public GameObject _startFloor;
    public GameObject _finishFloor;
    public Transform _parent;
    #endregion

    #region Private Variables
    private GameObject _floorPrefab;
    private int _floorCount;
    private List<GameObject> _floorPoolList = new List<GameObject>();
    #endregion

    /// <summary>
    /// Dep. Injection
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="poolManager"></param>
    [Inject]
    private void Installer(GameManager gameManager,PoolManager poolManager)
    {
        _gameManager = gameManager;
        _poolManager = poolManager;
        _floorPrefab = _gameManager._floorPrefab;

    }

    /// <summary>
    /// Prepate the level 
    /// Set the floor positions
    /// </summary>
    /// <param name="level"></param>
    public void PrepareLevel(int level)
    {
        _floorCount = CalculateFloorCount(level);
        CheckPoolCount();
        GetPoolList();
        for (int i = 0; i < _floorCount; i++)
        {
            _floorPoolList[i].SetActive(true);
        }
        SetFloorPositions();
        RegionManager.Instance.SetRegions();
    }
     
    /// <summary>
    /// Get the list of floor from pool manager
    /// </summary>
    private void GetPoolList()
    {
        _floorPoolList = _poolManager._awailableGameObjectsDict[_floorPrefab].ToList();
    }
    
    /// <summary>
    /// Calculate the floor count the set the height
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private int CalculateFloorCount(int level)
    {
        return (level / _floorByLevel)+1;
    }

    /// <summary>
    /// Check count to pooling
    /// </summary>
    private void CheckPoolCount()
    {
        if (_floorCount >= _floorPoolList.Count)
        {
            _poolManager.ExpandPool(_floorPrefab, 5, transform);
        }
    }

    /// <summary>
    /// Set floor positions
    /// at top = start floor
    /// at bottom = finish floor
    /// </summary>
    private void SetFloorPositions()
    {
        Vector3 firstPos = _startFloor.transform.position;
        for (int i = 0; i < _floorCount; i++)
        {
            _floorPoolList[i].transform.position = firstPos + (Vector3.down * (i + 1) * _floorSize);
        }
        _finishFloor.transform.position = firstPos + (Vector3.down * (_floorCount + 1) * _floorSize);
    }

    /// <summary>
    /// Reset the Floor
    /// reset walls
    /// </summary>
    public void ResetFloors()
    {
        foreach (GameObject gm in (from g in _floorPoolList where g.activeSelf select g ))
        {
            gm.GetComponent<Floor>().regionGenerator.InialitizeMap();
        }
    }

}
