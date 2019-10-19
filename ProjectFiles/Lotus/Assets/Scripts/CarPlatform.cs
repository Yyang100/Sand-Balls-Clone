using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Movable Car Platform
/// Change the platform position and start to move the car
/// </summary>
public class CarPlatform : Singleton<CarPlatform>
{

    public CarController carController;
    public int targetBallCount = 3;
    
    /// <summary>
    /// Start position
    /// </summary>
    public float normalPlatformPositionY = -0.5f;
    /// <summary>
    /// End position
    /// </summary>
    public float targetPlatformPositionY = -1.5f;

    //dep. injection
    private LevelManager levelManager;

    private int ballCount;
    

    /// <summary>
    /// Injection
    /// </summary>
    /// <param name="levelManager"></param>
    [Inject]
    private void Inialitize(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    /// <summary>
    /// Reset the platform
    /// </summary>
    public void ResetPlatform()
    {
        ballCount = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, normalPlatformPositionY, transform.localPosition.z);
        carController.ResetCar();
    }

    /// <summary>
    /// Set the position with ball count parameter
    /// </summary>
    private void SetPositionWithBall()
    {
        float pos = normalPlatformPositionY + ( (targetPlatformPositionY - normalPlatformPositionY) / (targetBallCount) )* Mathf.Clamp(ballCount, 0, targetBallCount);
        transform.localPosition = new Vector3(transform.localPosition.x, pos, transform.localPosition.z);
    }

    /// <summary>
    /// Increase ball count and change position
    /// </summary>
    public void IncreaseBallCount()
    {
        ballCount++;
        SetPositionWithBall();
        CheckLevelCompleted();
    }

    /// <summary>
    /// Chech whether level scompleted  or not
    /// </summary>
    private void CheckLevelCompleted()
    {
        if (ballCount >= targetBallCount)
        {
            CancelInvoke();
            Invoke("LevelCompleted", 2);
        }
    }

    /// <summary>
    /// Move the car when level completed
    /// </summary>
    private void LevelCompleted()
    {
        levelManager.LevelCompleted();
        carController.Move();
    }

}
