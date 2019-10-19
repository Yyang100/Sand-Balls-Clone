using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class LevelManager : MonoBehaviour
{
    #region Private Variables
    private GameManager _gameManager;
    private Player _player; //
    private FloorManager _floorManager;
    private BallManager _ballManager;
    private CameraController _cameraController;
    private UIManager _uIManager;
    private int _level;
    #endregion


    public int Level
    {
        get { return _level; }
        private set { _level = value; _player.Level = value; }
    }


    [Inject]
    private void Installer(GameManager gameManager,Player player,FloorManager floorManager,
                            BallManager ballManager,CameraController cameraController,
                            UIManager uIManager)
    { 
        _gameManager = gameManager;
        _player = player;
        _floorManager = floorManager;
        _ballManager = ballManager;
        _cameraController = cameraController;
        _uIManager = uIManager;
    }

    /// <summary>
    /// get the last level
    /// prepare the level to play
    /// </summary>
    public void LoadLevel()
    {
        Level = PlayerPrefManager.GetInt("level"); 
        _floorManager.PrepareLevel(Level);
        _ballManager.ResetBalls();
        _cameraController.StartFollowing(_floorManager._finishFloor.transform.position);
    }

    /// <summary>
    /// Level completed
    /// Clear the UI
    /// </summary>
    public void LevelCompleted()
    {
        PlayerPrefManager.SetInt("level", ++Level);
        _uIManager.OpenStageClearPanel();
    }

    /// <summary>
    /// Restart Level
    /// reset obstacles and walls
    /// </summary>
    public void RestartLevel()
    {
        LoadLevel();
        ObstacleManager.Instance.ResetObstacles();
        RegionManager.Instance.ResetRegions();
    }

    /// <summary>
    /// Load the next level
    /// </summary>
    public void LoadNextLevel()
    {
        RestartLevel();
    }

}
