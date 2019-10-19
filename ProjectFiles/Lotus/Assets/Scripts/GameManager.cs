using System;
using UnityEngine;
using Zenject;

/// <summary>
/// Initialize the game
/// start the game
/// </summary>
public class GameManager : MonoBehaviour
{

    #region Injection Variables
    private LevelManager _levelManager;
    private PoolManager _poolManager;
    private BallManager _ballManager;
    private FloorManager _floorManager;
    #endregion 

    #region Public Variables
    public GameObject _floorPrefab;
    public GameObject _ballPrefab;
    #endregion

    [Inject]
    private void Installer(LevelManager levelManager,PoolManager poolManager,BallManager ballManager,FloorManager floorManager)
    {
        _levelManager = levelManager;
        _poolManager = poolManager;
        _ballManager = ballManager;
        _floorManager = floorManager;
    }


    private void Start()
    {
        InitializeGame();
    }


    /// <summary>
    /// Pool the objects
    /// Load the Level
    /// </summary>
    private void InitializeGame()
    {
        PoolableObject ballPoolObject = new PoolableObject(_ballPrefab, 5,_ballManager.transform);
        _poolManager.Pool(ballPoolObject);
        PoolableObject floorPoolObject = new PoolableObject(_floorPrefab, 10, _floorManager.transform);
        _poolManager.Pool(floorPoolObject);

        _levelManager.LoadLevel();
    }


}
