using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

public class BallManager : MonoBehaviour
{
    #region Dep. Injection
    private PoolManager poolManager;
    private GameManager gameManager;
    #endregion

    #region Public Variables
    public Transform ballInitializePosition;
    public List<GameObject> ballPoolList = new List<GameObject>();
    #endregion

    /// <summary>
    /// Dep. Injection
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="poolManager"></param>
    [Inject]
    private void Initialize(GameManager gameManager,PoolManager poolManager)
    {
        this.gameManager = gameManager;
        this.poolManager = poolManager;
    }


    /// <summary>
    /// Reset balls position when level start again/new
    /// </summary>
    public void ResetBalls()
    {
        GetBallPoolList();
        foreach(GameObject gO in ballPoolList){
            gO.SetActive(true);
            gO.transform.position = ballInitializePosition.position;
        }
    }

    /// <summary>
    /// Get ball list from PoolManager
    /// </summary>
    private void GetBallPoolList()
    {
        ballPoolList = poolManager._awailableGameObjectsDict[gameManager._ballPrefab].ToList();
    }

}
