using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
 

namespace Injection
{
    /// <summary>
    /// Game Installer = using for injection
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
       
        public GameManager _gameManager;
        public LevelManager _levelManager;
        public Player _player;
        public FloorManager _floorManager;
        public RegionManager _regionManager;
        public BallManager _ballManager;
        public PoolManager _poolManager;
        public CameraController _cameraController;
        public UIManager uIManager;
        public InputManager _inputManager;
        public override void InstallBindings()
        {
            Container.BindInstance<GameManager>(_gameManager);
            Container.BindInstance<LevelManager>(_levelManager);
            Container.BindInstance<Player>(_player);
            Container.BindInstance<FloorManager>(_floorManager);
            Container.BindInstance<RegionManager>(_regionManager);
            Container.BindInstance<BallManager>(_ballManager);
            Container.BindInstance<PoolManager>(_poolManager);
            Container.BindInstance<CameraController>(_cameraController);
            Container.BindInstance<UIManager>(uIManager);
            Container.BindInstance<InputManager>(_inputManager); 
        }

         
    }
}